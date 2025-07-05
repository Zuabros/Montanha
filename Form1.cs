// MÓDULO 01 - IMPORTAÇÕES
using Discord; // garante acesso ao namespace da classe funcoes
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices; // Required for Windows API functions
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using static Discord.Form1;
using static Discord.funcoes;

namespace Discord
{
 public partial class Form1 : Form
 {
	// MÓDULO 02 - VARIÁVEIS GLOBAIS
	List<pixel> pixels = new List<pixel>();         // Pontos de leitura no addon
	List<point> points = new List<point>();         // entidades visíveis no minimapa
	List<loc> lwp = new List<loc>();                // waypoints de navegação
	byte[,] mini = new byte[256, 256];              // minimapa dinâmico (centrado no player)
	byte[,] map = new byte[1000, 1000];             // mapa da área inteira (fixo)
	int stuckcount = 0; // contador de ciclos consecutivos parado
	loc oldloc;         // última posição registrada pra comparação

	// --------------------------------
	// VARIÁVEIS TEMPORÁRIAS PARA CRIAÇÃO DE CERCAS
	// --------------------------------
	loc temp_cerca_a;           // extremidade A da cerca
	loc temp_cerca_b;           // extremidade B da cerca  
	loc temp_cerca_p;           // ponto proibido
	bool a_definido = false;    // controles de estado
	bool b_definido = false;
	bool p_definido = false;


	// VARIAVEIS DO MULTITHREAD
	private Thread updateThread;
	private volatile bool threadRunning = false;

	// VARIAVEIS DO DECAY
	DecaySession decay = new DecaySession();
	DecayTracker tracker = new DecayTracker();
	bool emCombate = false;


	// M20 - Variaveis de estado de jogo e personagem
	int res_y = Screen.PrimaryScreen.Bounds.Height;
	public bool on = true; // bot on
	public bool stopscan = false;
	int indexAtual = 0;

	// LISTA DE CERCA
	List<Cerca> lcercas = new List<Cerca>();  // lista de cercas

	// LISTA DE MOBS UNICOS QUE PARTICPARAM DO COMBATE, PARA DEFINIR QUANTIDADE DE LOOT E SKIN: 
	HashSet<int> killed_skin = new HashSet<int>();
	HashSet<int> killed_noskin = new HashSet<int>();


	//---------------------------------
	// DLL Imports
	//----------------------------------

	// IMPORT PARA CURSOR (CAPTURAR E DESENHAR CURSOR) 
	[DllImport("gdi32.dll", SetLastError = true)]
	public static extern int GetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan, uint cScanLines, [Out] byte[] lpvBits, ref BITMAPINFOHEADER lpbi, uint uUsage);


	// IMPORTS PARA CURSOR E DESENHO DE ÍCONES
	[DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr hWnd);
	[DllImport("user32.dll")] public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
	[DllImport("gdi32.dll")] public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
	[DllImport("gdi32.dll")] public static extern bool DeleteDC(IntPtr hdc);
	[DllImport("gdi32.dll")] public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
	[DllImport("gdi32.dll")] public static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);
	[DllImport("user32.dll")] public static extern bool DrawIconEx(IntPtr hDC, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, int istep, IntPtr hbrFlickerFreeDraw, int diFlags);


	[DllImport("gdi32.dll")]
	public static extern int GetObject(IntPtr hgdiobj, int cbBuffer, out BITMAP lpvObject); // para ler desenho do cursor


	[StructLayout(LayoutKind.Sequential)] // para ler desenho do cursor 
	public struct BITMAP // usado para ler o desenho do cursor 
	{
	 public int bmType;       // tipo (geralmente 0)
	 public int bmWidth;      // largura em pixels
	 public int bmHeight;     // altura em pixels
	 public int bmWidthBytes; // bytes por linha
	 public ushort bmPlanes;  // geralmente 1
	 public ushort bmBitsPixel; // profundidade (ex: 32 = RGBA)
	 public IntPtr bmBits;    // ponteiro pros bits da imagem (opcional)
	}

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

	[StructLayout(LayoutKind.Sequential)]
	public struct ICONINFO
	{
	 public bool fIcon;         // true se for ícone, false se for cursor
	 public int xHotspot;       // posição x do ponto clicável
	 public int yHotspot;       // posição y do ponto clicável
	 public IntPtr hbmMask;     // bitmap da máscara
	 public IntPtr hbmColor;    // bitmap da cor
	}


	[System.Runtime.InteropServices.DllImport("user32.dll")]
	static extern short GetAsyncKeyState(int vKey);

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
	 public Int32 x;
	 public Int32 y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CURSORINFO
	{
	 public Int32 cbSize;        // Specifies the size, in bytes, of the structure.
															 // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
	 public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
															 //    0             The cursor is hidden.
															 //    CURSOR_SHOWING    The cursor is showing.
															 //    CURSOR_SUPPRESSED    (Windows 8 and above.) The cursor is suppressed. This flag indicates that the system is not drawing the cursor because the user is providing input through touch or pen instead of the mouse.
	 public IntPtr hCursor;          // Handle to the cursor.
	 public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor.
	}
	[DllImport("user32.dll")]
	static extern bool GetCursorInfo(ref CURSORINFO pci);

	private const Int32 CURSOR_SHOWING = 0x00000001;
	private const Int32 CURSOR_SUPPRESSED = 0x00000002;
	public IntPtr getcursor()
	{
	 CURSORINFO pci = new CURSORINFO();                          // cria struct local
	 pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));           // seta o tamanho obrigatório
	 GetCursorInfo(ref pci);                                    // chama API do Windows
	 rtbcursor.Text += pci.hCursor.ToString() + " ";            // imprime o ponteiro
	 return pci.hCursor;                                        // retorna ponteiro
	}

	/// <summary>Must initialize cbSize</summary>




	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(IntPtr hWnd);
	

	[DllImport("user32.dll")]
	private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
	private const int MOUSEEVENTF_LEFTDOWN = 0x02;
	private const int MOUSEEVENTF_LEFTUP = 0x04;

	// Constantes de mouse e teclado

	public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
	public const int MOUSEEVENTF_RIGHTUP = 0x10;
	public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
	public const int KEYEVENTF_KEYUP = 0x0002;

	// --------------------------------------------
	// TECLAS BÁSICAS - NOMES DE TECLAS FÍSICAS
	// --------------------------------------------
	public const int TAB = 0x09; // tecla Tab
	public const int QKEY = 0x51; // tecla Q 
	public const int WKEY = 0x57; // tecla W 
	public const int EKEY = 0x45; // tecla E 
	public const int SKEY = 0x53; // tecla S 
	public const int IKEY = 0x49; // tecla I 
	public const int AKEY = 0x41; // tecla A 
	public const int DKEY = 0x44; // tecla D 
	public const int SPACEBAR = 0x20; // espaço
	public const int ENTER = 0x0D; // tecla ENTER 
	public const int BARRA = 0x6E; // NumPad Slash
	public const int IGUAL = 0xBB; // tecla "=" padrão (igual normal, perto do Backspace)

	// --------------------------------------------
	// TECLAS NUMÉRICAS SUPERIORES
	// --------------------------------------------
	public const int UM = 0x31; // tecla 1 
	public const int DOIS = 0x32; // tecla 2 
	public const int TRES = 0x33; // tecla 3 
	public const int QUATRO = 0x34; // tecla 4 
	public const int CINCO = 0x35; // tecla 5 
	public const int SEIS = 0x36; // tecla 6 
	public const int SETE = 0x37; // tecla 7 
	public const int OITO = 0x38; // tecla 8 
	public const int NOVE = 0x39; // tecla 9 
	public const int ZERO = 0x30; // tecla 0 

	// --------------------------------------------
	// TECLAS DO NUMPAD
	// --------------------------------------------
	public const int N0 = 0x60; // NumPad 0
	public const int N1 = 0x61; // NumPad 1 
	public const int N2 = 0x62; // NumPad 2 
	public const int N3 = 0x63; // NumPad 3 
	public const int N4 = 0x64; // NumPad 4 
	public const int N5 = 0x65; // NumPad 5 
	public const int N6 = 0x66; // NumPad 6 
	public const int N7 = 0x67; // NumPad 7 
	public const int N8 = 0x68; // NumPad 8 
	public const int N9 = 0x69; // NumPad 9 

	// --------------------------------------------
	// ALIAS DE CURSORES (identificação visual)
	// --------------------------------------------
	// os valores abaixo são gerados com checksum(img) = soma de todos os bytes do cursor % 256
	// o cursor é renderizado via DrawIconEx em um bitmap 32x32 invisível (32bpp BGRA)
	// -------------------------------------------------
	// EM HOMENAGEM AO ALEMÃO (paladino level 41)
	// Caiu ao clicar em um elite achando que era loot
	// Este método foi criado para que isso jamais se repita
	// -------------------------------------------------

	public const byte LOOT = 64;    // cursor de loot (saquinho)
	public const byte SKIN = 63;    // cursor de skinning (courinho)
	public const byte SKIN_GRAY = 44;    // skinning fora de alcance
	public const byte HAND = 79;    // cursor de interação (mãozinha)
	public const byte SWORD = 33;    // cursor de ataque (espadinha)
	public const byte SWORD_GRAY = 201;   // espadinha fora de alcance
	public const byte TRAIN = 50;    // cursor de trainer / livro
	public const byte TRAIN_GRAY = 62;    // trainer / livro fora de alcance
	public const byte HERB_GRAY = 254;   // herborismo fora de alcance
	public const byte BALLOON = 209;   // cursor de diálogo (balão de fala)
	public const byte BALLOON_GRAY = 239;   // balão de fala cinza
	public const byte TRADE = 233;   // cursor de troca
	public const byte TRADE_GRAY = 206;   // troca fora de alcance
	public const byte MAIL = 158;   // cursor de caixa de correio
	public const byte MAIL_GRAY = 223;   // caixa de correio fora de alcance
	public const byte GEAR = 26;    // engrenagem (banco, forja, etc.)
	public const byte GEAR_GRAY = 224;   // engrenagem fora de alcance
	public const byte HIDDEN = 0;     // cursor invisível (oculto pelo jogo)
	public const int HERB = 69;           // planta coletável (herbalismo)
	public const int MINE_GRAY = 201;     // veia de mineração cinza ou espadinha fora do alcance

	// --------------------------------------------
	// SKILLS GERAIS (comuns ou reutilizáveis)
	// --------------------------------------------
	public const int AUTOATTACK = INTERACT;     // ataque automático
	public const int CLEARTGT = SETE;   // limpa o target
	public const int TARGETLAST = OITO;   // retarget último inimigo
	public const int STONEFORM = NOVE;   // racial dos anões
	public const int HEARTHSTONE = N8;     // Hearthstone
	public const int HEALTHPOTION = N9;     // Poção de vida
	//public const int MANAPOTION = N0;     // Poção de mana
	public const int ANDA = WKEY;   // comando de andar
	public const int SLOW = F2;     // debuff de lentidão (genérico)
	public const int ASSIST = F6;     // debuff de lentidão (genérico)
	public const int GCD = 1501;  // Global Cooldown (tempo de recarga global em milissegundos)
	public const int FOOD = F12;     // ataque automático
	public const int STOPCAST = IGUAL;       // Stop Cast (para de lançar feitiço)
	public const int STOPATTACK = IGUAL;         // Stop Attack (para de atacar) MELEE

	// --------------------------------
	// ALIASES PRIEST
	// --------------------------------
	public const int SMITE = N1;        // Smite
	public const int PWS = N2;          // Power Word: Shield  
	public const int SWP = N3;          // Shadow Word: Pain
	public const int RENEW = N4;        // Renew
	public const int HEAL = N5;         // Lesser Heal/Heal
	public const int BIGHEAL = N6;      // Greater Heal
	public const int FORTITUDE = N7;    // Power Word: Fortitude
	public const int INNERFIRE = N0;    // Inner Fire
	public const int DESPERATE = F1;         // desperate prayer 
	public const int INNERFOCUS = F2;   // Inner Focus
	public const int DISPEL = F3;       // Dispel Magic
	public const int WAND = F4;         // Wand
	public const int DIVSPIRIT = F5;    // Divine Spirit
	public const int RESURRECT = F7;   // Resurrect
	public const int MINDCONTROL = F8;  // Mind Control

	// --------------------------------------------
	// SKILLS EXCLUSIVAS DO PALADINO
	// --------------------------------------------
	public const int SOR = DOIS;   // Seal of Righteousness
	public const int SOL = N3;     // Seal of Light
	public const int SOTC = F5;      // Seal of the Crusader
	public const int JUDGEMENT = CINCO;  // Judgement
	public const int HOJ = QUATRO; // Hammer of Justice
	public const int PURIFY = SEIS;   // remove poison/disease
	public const int HLIGHT = ZERO;   // Holy Light
	public const int EXORCISM = N6;     // Exorcism

	public const int BOM = TRES;   // Blessing of Might
	public const int BOW = N4;     // Blessing of Wisdom
	public const int BOK = N5;     // Blessing of Kings
	public const int BOSA = N7;     // Blessing of Sanctuary

	public const int DPROT = N1;     // Divine Protection
	public const int LOH = N2;     // Lay on Hands


	public const int DEVAURA = F3;     // Devotion Aura
	public const int RETAURA = F4;     // Retribution Aura
	public const int BOP = F7;     // Retribution Aura
	public const int CONAURA = F8;     // Concentration Aura
	public const int DSHIELD = F9;     // Divine Shield (Bubble)
	public const int FLASHHEAL = F10;     // Divine Shield (Bubble)

	// --------------------------------------------
	// SKILLS EXCLUSIVAS DO WARRIOR
	// --------------------------------------------
	public const int HEROICS = N1;  // Heroic Strike
	public const int OVERPOWER = N2;  // Overpower
																		//public const int THROW      = N3;  // Arremesso (Throw) já tem no rogue, usa a mesma
	public const int HAMSTRING = N4;  // Hamstring
	public const int BATTLESHOUT = N5;  // Battle Shout
	public const int BLOODRAGE = N6;  // Bloodrage
	public const int THUNDERCLAP = N7;  // Thunder Clap
	public const int SUNDERARMOR = N0;  // Sunder Armor
	public const int CLEAVE = DOIS;  // Cleave
	public const int DEMORALIZING = F1;  // Demoralizing Shout
	public const int SLAM = F2;  // Death Wish
	public const int REND = F3;  // Rend
	public const int RETALIATION = F4;  // Retaliation
	public const int CHARGE = F5;  // Charge
	public const int EXECUTE = F6;  // Execute


	// --------------------------------------------
	// SKILLS EXCLUSIVAS DO WARLOCK
	// --------------------------------------------
	public const int CREATE_HEALTHSTONE = DOIS; // Create Healthstone (cria pedra de saúde)

	public const int SHADOWBOLT = N1;        // Shadow Bolt (spell principal)
	public const int IMMOLATE = N2;          // Immolate (DoT + dano inicial)
	public const int CORRUPTION = N3;        // Corruption (DoT puro)
	public const int CURSEWEAKNESS = N4;     // Curse of Weakness (reduz ataque)
	public const int CURSEAGONY = N5;        // Curse of Agony (DoT crescente)
	public const int DRAINSOUL = N6;         // Drain Soul (channel + soul shard)
	public const int DRAINLIFE = N7;         // Drain Life (channel + cura)

	public const int HEALTHFUNNEL = N0;       // Stop Cast (para de lançar feitiço)

	public const int PETATTACK = F1;         // Summon Imp
	public const int SUMMONPET = F2;         // Summon Imp
	public const int DEMONSKIN = F3;         // Summon Imp
	//public const int WAND = F4;         // ja tem no priest 
	public const int LIFETAP = F5;         // Summon Imp
	// F6 = Assist (pet) 
 public const int HEALTHSTONE = F7;         // Summon Imp
	public const int SIPHONLIFE = F1;     // Siphon Life (DoT + cura)

	// ============================================
	// SKILLS EXCLUSIVAS DO HUNTER (VERSÃO EXPANDIDA)
	// Adicionar esta seção após as outras classes (após ROGUE e antes de TIPOS DE CRIATURAS)
	// ============================================

	public const int RAPTORS = N1;           // Raptor Strike (análogo ao HEROICS do Warrior)
	public const int AUTOSHOT = N2;          // Auto Shot (skill principal à distância)
	public const int MONGOOSE = N3;          // Mongoose Bite (ataque especial de pet)
	public const int HUNTERSMARK = N4;       // Hunter's Mark (para futuras implementações)

	// NOVAS SKILLS ADICIONADAS
	public const int SERPENTSTING = N5;      // Serpent Sting (DoT venenoso)
	public const int ARCANESHOT = N6;        // Arcane Shot (tiro instantâneo)
	public const int MULTISHOT = N7;         // Multi-Shot (para futuras implementações)
	public const int CONCUSSIVESHOT = N0;    // Concussive Shot (slow/desacelera)

	// Pet commands (usar o mesmo PETATTACK do Warlock)
	// public const int PETATTACK = F1;     // já definido no Warlock - reutilizar
	// F6 = ASSIST (já definido globalmente)

	// Aspects (para futuras implementações)
	// public const int ASPECTHAWK = F3;    // Aspect of the Hawk
	// public const int ASPECTCHEETAH = F4; // Aspect of the Cheetah
	// public const int ASPECTMONKEY = F5;  // Aspect of the Monkey

	// NOTA: SUMMONPET deve usar a mesma tecla do Warlock para consistência
	// public const int SUMMONPET = F2;     // já definido no Warlock - reutilizar

	// --------------------------------------------
	// SKILLS EXCLUSIVAS DO ROGUE
	// --------------------------------------------
	//public const int STOPATTACK = IGUAL;         // Stop Attack (para de atacar) 
	public const int SS = N1;               // Sinister Strike
	public const int EVIS = N2;             // Eviscerate
	public const int THROW = N3;            // Throwing Knife
	public const int STEALTH = N4;          // Stealth
	public const int PICKPOCKET = N5;       // Pickpocket
	public const int EVASION = N6;          // Evasion
	public const int SAD = N7;              // Slice and Dice
	public const int EXPOSE_ARMOR = N0;     // Expose Armor
	
	public const int KICK = F1;             // Kick
	public const int VANISH = F2;           // Vanish
	public const int GOUGE = F3;            // Gouge
	public const int RIPOSTE = F4;          // Riposte
	public const int RUPTURE = F5;          // Rupture
	// F6 reservado para Assist

	// --------------------------------------------
	// TIPOS DE CRIATURAS
	// --------------------------------------------
	public const int HUMANOID = 50;  // humanoide
	public const int BEAST = 100; // besta
	public const int PLAYER_MELEE = 105; // player mané ogro (não-caster)
	public const int PLAYER_CASTER = 110; // player mané caster
	public const int CRITTER = 80;  // criatura pequena (critter)
	public const int OOZE = 90;  // lodo, gosma (Ooze)
	public const int ABERRATION = 95;  // aberração
	public const int UNDEAD = 150; // morto-vivo
	public const int DEMON = 200; // demônio
	public const int ELEMENTAL = 210; // elemental
	public const int MECHANICAL = 220; // mecânico
	public const int DRAGONKIN = 230; // dragonete
	public const int GIANT = 240; // gigante
	public const int TOTEM = 2;   // totem
	public const int NONCOMBAT_PET = 3;   // mascote não-combatente
	public const int GAS_CLOUD = 4;   // nuvem de gás
	public const int MURLOC = 75;   // Murloc

	// --------------------------------------------
	// CÓDIGOS DE CLASSE (pixel 6 canal G)
	// --------------------------------------------
	public const int PALADIN = 255; // paladino
	public const int WARRIOR = 250; // guerreiro
	public const int HUNTER = 245; // caçador
	public const int ROGUE = 240; // ladino
	public const int PRIEST = 235; // sacerdote
	public const int SHAMAN = 230; // xamã
	public const int MAGE = 225; // mago
	public const int WARLOCK = 220; // bruxo
	public const int DRUID = 215; // druida

	// --------------------------------------------
	// UTILIDADES
	// --------------------------------------------
	public const int PULA = SPACEBAR;  // pula
	public const int INTERACT = IKEY;      // interage com o alvo



	// --------------------------------------------
	// F KEYS
	// --------------------------------------------
	public const int F1 = 0x70; // tecla F1
	public const int F2 = 0x71; // tecla F2 // WALK 
	public const int F3 = 0x72; // tecla F3 
	public const int F4 = 0x73; // tecla F4 
	public const int F5 = 0x74; // tecla F5 
	public const int F6 = 0x75; // tecla F6 
	public const int F7 = 0x76; // tecla F7 
	public const int F8 = 0x77; // tecla F8 
	public const int F9 = 0x78; // tecla F9
	public const int F10 = 0x79; // tecla F10
	public const int F11 = 0x7A; // tecla F11
	public const int F12 = 0x7B; // tecla F12

	// --------------------------------------------
	// CLASSES  E STRUCTS 
	// --------------------------------------------
	// =====================================================
	// SISTEMA DE OTIMIZAÇÃO ADAPTATIVA PARA COMBO POINTS
	// =====================================================

	// =====================================================
	// SISTEMA DE OTIMIZAÇÃO ADAPTATIVA PARA COMBO POINTS
	// =====================================================

	// Variáveis globais para o sistema de otimização
	public static class ComboOptimizer
	{
	 // Probabilidades lidas das textboxes (com fallback)
	 public static double prob_regular = 33.0;
	 public static double prob_plus1 = 33.0;
	 public static double prob_minus1 = 34.0;

	 // Totais acumulados por categoria (sem reset)
	 public static int total_energy_regular = 0;
	 public static int total_dano_regular = 0;
	 public static int total_energy_plus1 = 0;
	 public static int total_dano_plus1 = 0;
	 public static int total_energy_minus1 = 0;
	 public static int total_dano_minus1 = 0;

	 // Estado atual
	 public static int categoria_atual = 0; // 0=regular, 1=plus1, -1=minus1
	 public static int contador_casts = 0; // para ajustar probabilidades a cada 10 casts

	 // Referência para o form (para acessar textboxes)
	 public static Form1 form_instance = null;

	 // Lê probabilidades das textboxes (com fallback para valores padrão)
	 public static void LerProbabilidades()
	 {
		try
		{
		 if (form_instance != null)
		 {
			prob_regular = double.Parse(form_instance.tb_prob_zero.Text);
			prob_plus1 = double.Parse(form_instance.tb_prob_plus1.Text);
			prob_minus1 = double.Parse(form_instance.tb_prob_minus1.Text);

			// Verifica se somam aproximadamente 100%
			double total = prob_regular + prob_plus1 + prob_minus1;
			if (Math.Abs(total - 100.0) > 5.0) // tolerância de 5%
			{
			 throw new Exception("Probabilidades não somam 100%");
			}
		 }
		}
		catch
		{
		 // Em caso de erro, reseta para valores padrão
		 prob_regular = 33.0;
		 prob_plus1 = 33.0;
		 prob_minus1 = 34.0;
		 AtualizarTextboxesProbabilidades();
		}
	 }

	 // Atualiza as textboxes de probabilidades
	 public static void AtualizarTextboxesProbabilidades()
	 {
		if (form_instance != null)
		{
		 form_instance.tb_prob_zero.Text = prob_regular.ToString("F0");
		 form_instance.tb_prob_plus1.Text = prob_plus1.ToString("F0");
		 form_instance.tb_prob_minus1.Text = prob_minus1.ToString("F0");
		}
	 }

	 // Determina qual categoria usar no próximo combate
	 public static int EscolherCategoria()
	 {
		// Lê probabilidades das textboxes sempre
		LerProbabilidades();

		Random rand = new Random();
		double sorteio = rand.NextDouble() * 100;

		if (sorteio <= prob_regular)
		 return 0; // regular
		else if (sorteio <= prob_regular + prob_plus1)
		 return 1; // plus1
		else
		 return -1; // minus1
	 }

	 // Calcula média atual de uma categoria
	 public static double CalcularMedia(int total_dano, int total_energy)
	 {
		if (total_energy == 0) return 0;
		return (total_dano * 100.0) / total_energy;
	 }

	 // Registra cast (SS ou EVIS) na categoria atual
	 public static void RegistrarCast(int energia, int dano)
	 {
		switch (categoria_atual)
		{
		 case 0: // regular
			total_energy_regular += energia;
			total_dano_regular += dano;
			break;
		 case 1: // plus1
			total_energy_plus1 += energia;
			total_dano_plus1 += dano;
			break;
		 case -1: // minus1
			total_energy_minus1 += energia;
			total_dano_minus1 += dano;
			break;
		}

		contador_casts++;

		// Atualiza displays sempre
		AtualizarDisplays();

		// Atualiza probabilidades a cada 2 casts
		if (contador_casts % 2 == 0)
		{
		 AtualizarProbabilidades();
		}
	 }

	 // Atualiza as probabilidades baseado nos resultados (a cada 10 casts)
	 public static void AtualizarProbabilidades()
	 {
		// Só atualiza se todas as categorias têm dados
		if (total_energy_regular == 0 || total_energy_plus1 == 0 || total_energy_minus1 == 0)
		 return;

		// Calcula médias atuais
		double media_regular = CalcularMedia(total_dano_regular, total_energy_regular);
		double media_plus1 = CalcularMedia(total_dano_plus1, total_energy_plus1);
		double media_minus1 = CalcularMedia(total_dano_minus1, total_energy_minus1);

		// Lê probabilidades atuais das textboxes
		LerProbabilidades();

		// Monta lista com probabilidades e categorias
		var probs = new List<(double media, double prob, int cat)>
	{
		(media_regular, prob_regular, 0),
		(media_plus1, prob_plus1, 1),
		(media_minus1, prob_minus1, -1)
	};

		// Ordena do maior para o menor (por média de dano)
		probs.Sort((a, b) => b.media.CompareTo(a.media));

		// ================================
		// AUMENTA A MELHOR (até 90)
		// ================================
		probs[0] = (probs[0].media, Math.Min(probs[0].prob + 1, 90), probs[0].cat);

		// ================================
		// DIMINUI A PIOR DISPONÍVEL (>10)
		// ================================
		// Se o último já for 10, tenta o segundo pior
		if (probs[2].prob > 10)
		{
		 probs[2] = (probs[2].media, probs[2].prob - 1, probs[2].cat);
		}
		else if (probs[1].prob > 10)
		{
		 probs[1] = (probs[1].media, probs[1].prob - 1, probs[1].cat);
		}
		// Se todos em 10, não tira nada

		// ================================
		// APLICA DE VOLTA NAS VARIÁVEIS
		// ================================
		foreach (var p in probs)
		{
		 if (p.cat == 0) prob_regular = p.prob;
		 else if (p.cat == 1) prob_plus1 = p.prob;
		 else if (p.cat == -1) prob_minus1 = p.prob;
		}

		// ================================
		// NORMALIZA PARA SOMAR 100%
		// ================================
		double total = prob_regular + prob_plus1 + prob_minus1;
		prob_regular = (prob_regular / total) * 100;
		prob_plus1 = (prob_plus1 / total) * 100;
		prob_minus1 = (prob_minus1 / total) * 100;

		// Atualiza textboxes
		AtualizarTextboxesProbabilidades();
	 }
	 // Atualiza as textboxes com as médias
	 public static void AtualizarDisplays()
	 {
		if (form_instance != null)
		{
		 form_instance.tb_regular.Text = CalcularMedia(total_dano_regular, total_energy_regular).ToString("F1");
		 form_instance.tb_plus1.Text = CalcularMedia(total_dano_plus1, total_energy_plus1).ToString("F1");
		 form_instance.tb_minus1.Text = CalcularMedia(total_dano_minus1, total_energy_minus1).ToString("F1");
		}
	 }
	}




	// M10 - STRUCT LOC - REPRESENTA UMA POSIÇÃO (X, Y) NA ESCALA MULTIPLICADA POR 10.
	public struct loc
	{
	 public int x;
	 public int y;

	 public loc(int x, int y)
	 {
		this.x = x;
		this.y = y;
	 }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BITMAPINFOHEADER // usada pa ler o bitmap do cursor
	{
	 public uint biSize;          // tamanho da struct (40)
	 public int biWidth;          // largura (32)
	 public int biHeight;         // altura (32)
	 public short biPlanes;       // planos (1)
	 public short biBitCount;     // bits por pixel (32)
	 public uint biCompression;   // compressão (0 = BI_RGB)
	 public uint biSizeImage;     // tamanho em bytes (pode ser 0 com BI_RGB)
	 public int biXPelsPerMeter;  // resolução horizontal
	 public int biYPelsPerMeter;  // resolução vertical
	 public uint biClrUsed;       // paleta (0)
	 public uint biClrImportant;  // cores importantes (0)
	}

	// --------------------------------
	// MÓDULO 12 - STRUCT ELEMENT - REPRESENTA QUALQUER ENTIDADE (PLAYER, MOB, NPC) NO MAPA.
	// --------------------------------
	// Construtor
	// --------------------------------
	// INICALIZA O PROGRAMA (AUTOEXEC)
	// --------------------------------
	public Form1()
	{


	 InitializeComponent();
	 ComboOptimizer.form_instance = this;

	 
	 // DEFINE MONITOR DA DIREITA PARA NASCER----
	 // Para aparecer no monitor da direita (assumindo que seja o segundo monitor):
	 Screen[] screens = Screen.AllScreens;
	 if (screens.Length > 1)
	 {
		this.DesktopLocation = screens[0].WorkingArea.Location;
	 }
	 else
	 {
		this.StartPosition = FormStartPosition.CenterScreen;
	 }

	 inicializa();
	 //load_settings(); // carrega configuração do arquivo

	 load_settings(); // carrega configuração do arquivo
	 carregar_waypoints(); // Chama o carregamento automático	 
	 tab_nav.SelectedIndex = 0; // Seleciona a tabPage2 (índice 1) por padrão - debug purposes
	 lb_log.Clear(); // limpa todos os itens da ListBox log
	 lb_combatlog.Clear(); // limpa todos os itens da ListBox combatlog
	 carrega_loot(); // carrega frequencia de pontos de loot 
	 checkme(); // verifica se o player está em combate e ativa o botao de stop
	 if (me.level >=12) cb_nohumanoid.Checked = true; // ativa filtro de humanoides por padrão
	 killstats("load"); // carrega estatísticas de kills do arquivo




	 // ARRUMA INTERFACE---------------
	}
	// --------------------------------
	// CLASSE PIXEL (capturado do addon)
	// --------------------------------
	public class pixel
	{
	 public int x;
	 public int y;
	 public int r;
	 public int g;
	 public int b;

	 public pixel(int x, int y)
	 {
		this.x = x;
		this.y = y;
		this.r = 0;
		this.g = 0;
		this.b = 0;
	 }
	}


	// M21 - INICIALIZA VARIÁVEIS DINÂMICAS INICIAIS
	private void inicializa()
	{

	 
	 // CARREGA COORDENADAS DEFINE LOCAL DOS QUADRADINHOS 
	 int res_y = Screen.PrimaryScreen.Bounds.Height;
	 

	 // --------------------------------
	 // M17 - MÉTODO CALIBRARPIXELS - DETECTA PRATOS E DEFINE Y CENTRAL DOS HAMBÚRGUERES
	 // --------------------------------
	 calibrarpixels(pixels);


	 // CODIFICAÇÃO NO ADDON

	 // ----------------------------------------
	 // PIXEL 0 – STATUS DO PLAYER E TARGET
	 // ----------------------------------------
	 // R: Bitflags combinadas do player
	 //     128 = autoattack ativo
	 //      64 = healing potion pronta (no inventário e cooldown zerado)
	 //      32 = debuff Dazed
	 //       8 = em melee range
	 //    1-2-4= número de mobs batendo (cap 7)
	 // G: 0 a 127 HP atual do player (0–127)
	 //    128 = main hand ou offhand quebrada
	 // B: target id (guid simplificada) 


	 // ----------------------------------------
	 // PIXEL 1 – INVENTÁRIO, ERROS E RECURSO
	 // ----------------------------------------
	 //R: slots livres(3 bits) +flags(5 bits):
	 //bits 0–2(0–7) = slots livres nas bags(máximo 7)
	 //    bit 3(8) = wand_up(Shoot disponível e em range)
	 //    bits 4–7 = Wand global cooldown (0 a 1.5)
	 //
	 // G: flags de erro de combate:
	 //    bit 7 (128) = de costas (Facing wrong way)
	 //    bit 6 ( 64) = fora de alcance (Out of range)
	 //    0 = normal
	 //
	 // B: recurso primário + wand:
	 //    bits 0–6 (0–127) = recurso primário proporcional (mana, rage, energia, etc.)
	 //    bit 7 (128) = wand ativa (auto repeat Shoot)


	 // ----------------------------------------
	 // PIXEL 2 – POSIÇÃO X
	 // ----------------------------------------
	 // R: parte inteira de X dividido por 10 × 25
	 // G: parte decimal de X × 2.5
	 // B: parte inteira de X módulo 10 × 25


	 // ----------------------------------------
	 // PIXEL 3 – POSIÇÃO Y
	 // ----------------------------------------
	 // R: parte inteira de Y dividido por 10 × 25
	 // G: parte decimal de Y × 2.5
	 // B: parte inteira de Y módulo 10 × 25


	 // ----------------------------------------
	 // PIXEL 4 – FACING, VELOCIDADE E RACIAL
	 // ----------------------------------------
	 // R: Facing (0 a 1) × 255
	 // G: 255 - velocidade do player
	 // B: Bitflags:
	 //     bits 0–5 = cooldown do Stoneform (0–63)
	 //     bit 6 (64) = racial disponível (Stoneform ou Shadowmeld)
	 //     bit 7 (128) = Furbolg Form ativo


	 // ----------------------------------------
	 // PIXEL 5 – STATUS DE COMBATE E AMBIENTE
	 // ----------------------------------------
	 // R: Bitflags:
	 //     128 = em combate
	 //      64 = respirando (baixo d’água)
	 //      32 = comendo ou bebendo
	 // G: Progresso do cast do target (0–255)
	 // B: Debuffs no player, codificados:
	 //     1 = Curse | 2 = (genérico) | 4 = Poison | 8 = Disease | 16 = Magic
	 //     Resultado é somado e multiplicado por 8 (mantido compatível)


	 // ----------------------------------------
	 // PIXEL 6 – LEVEL, CLASSE E CAST DO PLAYER
	 // ----------------------------------------
	 // R: Level do player × 4 (cap implícito em 255)
	 // G: Classe:
	 //     255 = Paladino | 240 = Rogue | etc.
	 // B: Progresso do cast do player (0–255)


	 // ----------------------------------------
	 // PIXEL 7 – STATUS DO TARGET
	 // ----------------------------------------
	 // R: HP atual do target (0–255)
	 // G: Bitflags:
	 //     128 = target existe
	 //      64 = raro
	 //      32 = elite, rareelite ou worldboss
	 // B: Level do target × 4


	 // ----------------------------------------
	 // PIXEL 8 – TIPO DO TARGET
	 // ----------------------------------------
	 // R: Tipo codificado:
	 //     50 = Humanoide | 100 = Besta | 105 = Player melee | 110 = Caster
	 //     150 = Undead | 200 = Demônio | 210 = Elemental | 220 = Mecânico
	 //     230 = Dragonete | 240 = Gigante | 80 = Critter
	 // G: (livre)
	 // B: (livre)


	 // ----------------------------------------
	 // PIXEL 9 – REAÇÃO E AMEAÇA DO TARGET (bitflags)
	 // ----------------------------------------
	 // R: bits:
	 //    +1   = hostil
	 //    +2   = amigável
	 //    +4   = neutro
	 //    +8   = threat leve (t == 1)
	 //    +16  = threat médio (t == 2)
	 //    +32  = threat máximo (t == 3)
	 // bit 6(+64) = aggro no pet 
	 // G: livre
	 // B: livre

	 // ----------------------------------------
	 // PIXEL 11 (Paladino) – Vazio
	 // ----------------------------------------
	 // R, G, B = 0


	 // ----------------------------------------
	 // PIXEL 12 (Paladino) – Seals, Judgements e CDs
	 // ----------------------------------------
	 // R: Bitflags dos seals ativos:
	 //     128 = SOR | 64 = SOTC | 32 = SOJ | 16 = SOL | 8 = SOW | 4 = SOC
	 // G: Bitflags de judgements no target:
	 //     128 = Forbearance | 64 = JOTC | 32 = JOJ | 16 = JOL | 8 = JOW
	 // B: Bitflags de cast disponíveis:
	 //     128 = Lay on Hands | 64 = BoP | 32 = Bubble | 16 = Divine Protection


	 // ----------------------------------------
	 // PIXEL 13 (Paladino) – Blessings e HOJ
	 // ----------------------------------------
	 // R: Bitflags de blessings ativos:
	 //     1 = Salvation | 2 = Light | 4 = Freedom | 8 = Wisdom
	 //     16 = Protection | 32 = Kings | 64 = Might | 128 = Sanctuary
	 // G: 128 se HOJ estiver pronto (CD = 0)
	 // B: 255 se HOJ em alcance


	 // ----------------------------------------
	 // PIXEL 14 (Paladino) – Vazio
	 // ----------------------------------------
	 // R, G, B = 0


	 // ----------------------------------------
	 // PIXEL 15 (Paladino) – Vazio
	 // ----------------------------------------
	 // R, G, B = 0


	 // ----------------------------------------
	 // ROGUE
	 // ----------------------------------------
	 // PIXEL 10 (Rogue) – Combo Points + CDs + Auras + Debuffs
	 // ----------------------------------------
	 // R: bits 0–2 = número de combo points (0 a 7)
	 //     bit 0 = +1
	 //     bit 1 = +2
	 //     bit 2 = +4
	 //     bit 3 = throw_up         (Throw ou Shoot pronto e em range)
	 //     bit 4 = stealth_up       (pode ativar Stealth agora)
	 //     bit 5 = evasion_up       (Evasion pronto)
	 //     bit 6 = sad_up           (Slice and Dice pronto)
	 //     bit 7 = expose_up        (Expose Armor pronto + energia ≥ 25)

	 // G: bit 0 = has_stealth       (aura de Stealth ativa)
	 //     bit 1 = has_sad          (aura Slice and Dice ativa)
	 //     bit 2 = kick_up          (Kick pronto e em range)

	 // B: bit 0 = has_expose        (target com debuff Expose Armor)
	 //     bit 1 = vanish_up        (Vanish pronto)
	 //     bit 2 = riposte_up       (Riposte pronto)
	 //     bit 3 = gouge_up         (Gouge pronto)
	 //     bit 4 = rupture_up       (Rupture pronto + energia suficiente)
	 //     bit 5 = has_rupture      (target com debuff Rupture)


	 // ----------------------------------------
	 // PIXEL 11 a 15 (Rogue) – Vazios por enquanto
	 // ----------------------------------------
	 // R, G, B = 0

	 // -------------------------------------------
	 // PIXEL 10 – WARRIOR (status específicos)
	 // -------------------------------------------
	 // R: bit 0 = charge_up        (Charge pronto e em range)
	 // R: bit 1 = throw_up         (Throw ou Shoot Crossbow pronto e em range)
	 // R: bit 2 = bs_up            (Battle Shout pronto e rage >= 10)
	 // R: bit 3 = cleave_up        (Cleave pronto e rage >= 20)
	 // R: bit 4 = demo_up          (Demoralizing Shout pronto e rage >= 10)
	 // R: bit 5 = rend_up          (Rend pronto e rage >= 10)
	 // R: bit 6 = thun_up          (Thunder Clap pronto e rage >= 20)
	 // R: bit 7 = hs_up            (Heroic Strike pronto e rage >= 15)

	 // G: bit 0 = has_cleave       (modo Cleave ativado)
	 // G: bit 1 = ovp_up           (Overpower pronto)
	 // G: bit 2 = bloodrage_up     (Bloodrage pronto)
	 // G: bit 3 = hams_up          (Hamstring pronto)
	 // G: bit 4 = retaliate_up     (Retaliation pronto)
	 // G: bit 5 = dwish_up         (Death Wish pronto)
	 // G: bit 6 = has_bs           (Battle Shout ativo no player)
	 // G: bit 7 = has_rend         (Rend ativo no target)

	 // B: bit 0 = hs_casting       (Heroic Strike enfileirado como autoattack)
	 // B: bit 1 = little_sundered  (target com menos de 5 stacks de Sunder Armor)
	 // B: bit 2 = execute_up       (Execute pronto e em melee range)
	 // B: bit 3 = has_demoralizing (target com debuff Demoralizing Shout)
	 // B: bit 4 = sunder_up        (Sunder Armor pronto, em range e com rage)
	 // -------------------------------------------


	 // -------------------------------------------


	}
	// --------------------------------
	// M17 - MÉTODO CALIBRAPIXELS - DETECTA PRATOS E CALCULA HAMBÚRGUERES ENTRE ELES
	// --------------------------------
	void calibrarpixels(List<pixel> pixels)
{
focawow();                                      // foca a janela do jogo
int x = 2;                                      // coluna fixa dos pixels
List<int> pratos = new List<int>();              // lista de y dos pratos
int lastprato = 5000;                           // último prato (inicial fora de alcance)
int max_y = res_y - 1;                           // maior y (topo da tela)
bool primeiro_carne = false;                    // flag: primeiro pixel é carne?
int nao_prato_count = 0;                        // contador de não-pratos consecutivos
int limite_nao_prato = 10;                      // limite para encerrar busca
	 

// verifica se o maior y é carne
Color c = GetColorAt(x, max_y);                 // lê cor do topo
if (!isprato(c))                                // não é prato?
 primeiro_carne = true;                      // marca como carneGetColorAt

// varre pixels de cima pra baixo
for (int y = max_y; y >= max_y - 100; y--)      // começa no topo até 100 pixels abaixo
{
 c = GetColorAt(x, y);                       // lê cor atual
 if (isprato(c))                              // é prato?
 {
	if (Math.Abs(lastprato - y) > 1)         // diferença maior que 1?
	{
	 pratos.Add(y);                      // adiciona prato
	 lastprato = y;                      // atualiza último prato
	 if (false )
		loga("prato cor " + c.R + "," + c.G + "," + c.B + " em y=" + y);
	}
	else
	 if (false) loga("prato " + y + " ignorado");    // prato duplicado
	nao_prato_count = 0;                    // reseta contador de não-pratos
 }
 else                                         // não é prato
 {
	nao_prato_count++;                      // incrementa contador de não-pratos
	if (nao_prato_count >= limite_nao_prato) // atingiu o limite?
	{
	 if (false )
		loga("encerrando busca por não encontrar pratos por " + limite_nao_prato + " pixels.");
	 break;                              // encerra a busca
	}
 }
}

// calcula hambúrgueres
pixels.Clear();                                   // limpa lista de pixels
int count = 0;                                   // contador de hambúrgueres

// primeiro hambúrguer
int y1 = primeiro_carne ? max_y : pratos[0];    // topo: max_y ou primeiro prato
int y2 = pratos[primeiro_carne ? 0 : 1];         // próximo prato
int ycarne = (int)Math.Round((y1 + y2) / 2.0);    // centro da carne
pixels.Add(new pixel(x, ycarne));               // adiciona hambúrguer
count++;                                        // incrementa contador
if (false ) // codigo desabilitado
{
 c = GetColorAt(x, ycarne);                   // lê cor da carne
 loga("hamburger " + count.ToString("d2") + ": cor " + c.R + "," + c.G + "," + c.B + " calculado em y=" + ycarne);
}

// demais hambúrgueres
for (int i = primeiro_carne ? 0 : 1; i < pratos.Count - 1; i++)
{
 y1 = pratos[i];                             // prato inferior
 y2 = pratos[i + 1];                         // prato superior
 ycarne = (int)Math.Round((y1 + y2) / 2.0);    // centro da carne
 pixels.Add(new pixel(x, ycarne));           // adiciona hambúrguer
 count++;                                    // incrementa contador
 if (false) // codigo desabilitado pra nao sujar interface
 {
	c = GetColorAt(x, ycarne);               // lê cor da carne
	loga("hamburger " + count.ToString("d2") + ": cor " + c.R + "," + c.G + "," + c.B + " calculado em y=" + ycarne);
 }
}

// log final (removido para evitar logs desnecessários)
// loga(count + " hamburgeres localizados entre " + pratos.Count + " pratos detectados.");
}


// --------------------------------
// M19 - MÉTODO READPIXELS - LÊ CORES DOS HAMBÚRGUERES E ATUALIZA A LISTA
// --------------------------------
void readpixels(List<pixel> pixels)
{
focawow();                     // foca a janela do WoW

for (int i = 0; i < pixels.Count; i++)       // percorre todos os pixels
{
 var p = pixels[i];              // pega referência ao pixel
 Color cor = GetColorAt(p.x, p.y);       // lê a cor da tela naquele ponto

 pixels[i].r = cor.R;            // salva canal vermelho
 pixels[i].g = cor.G;            // salva canal verde
 pixels[i].b = cor.B;            // salva canal azul

 if (false)  // se debug e loga estiverem marcados
	loga("hamburger " + i + ": R=" + p.r + " G=" + p.g + " B=" + p.b); // mostra cor lida
}
}
	// --------------------------------
	// M16 - MÉTODO GETSTATS - CAPTURA STATUS DO PERSONAGEM
	// --------------------------------
	public void getstats(ref element e)
	{
	 readpixels(pixels);

	 // ----------------------------------------
	 // PIXEL 0 – STATUS DO PLAYER E TARGET
	 // ----------------------------------------
	 // R: Bitflags combinadas do player
	 //     128 = autoattack ativo
	 //      64 = healing potion pronta
	 //      32 = debuff Dazed
	 //       8 = em melee range
	 //    1-2-4 = número de mobs batendo (0–7)
	 // G: 0 a 127 = HP atual (0–100%)
	 //    128     = arma principal ou offhand quebrada
	 // B: (vazio)
	 // ----------------------------------------

	 // separa flag de arma quebrada (bit 7 do G)
	 me.armabroken = (pixels[0].g & 128) != 0;

	 // extrai bits 0–6 (valor do HP) e converte em % (0–100)
	 int raw_hp = pixels[0].g & 127;
	 int v_hp = (raw_hp * 100) / 127; // usa 127 porque é o valor máximo real
	 e.hp = v_hp;

	 // morreu: se HP chegou a 0
	 e.morreu = (e.hp == 0);

	 // debuff Dazed = bit 32
	 me.dazed = (pixels[0].r & 32) != 0;

	 // autoattack = bit 128
	 e.autoattack = (pixels[0].r & 128) != 0;

	 // healing potion pronta = bit 64
	 e.hp_potion_rdy = (pixels[0].r & 64) != 0;

	 // melee range = bit 8
	 me.melee = (pixels[0].r & 8) != 0;

	 // número de mobs batendo = bits 0 a 2
	 int mobs = pixels[0].r & 7;
	 if (me.combat && mobs != me.mobs)
	 {
		if (mobs > me.mobs && me.hp < 90 && mobs > 1)
		{
		 loga("Chegaram novos convidados na festa!");
		 loga("Dando backpedal para evitar dar as costas.");
		 if (!tar.casting && !cb_no_backpedal.Checked) aperta(SKEY, 1000); // backpedal 1.2s

		}
		loga("Mobs: " + mobs);
	 }
	 me.mobs = mobs;

	 // debug visual
	 tb_hp.Text = v_hp.ToString();

	 // BLUE - TARGET "unique" ID (GUID HASH)
	 tar.id = pixels[0].b; // id semi-único do target vindo do pixel 0B

	 // ------------------------------------------
	 // Pixel 1 - Recurso (canal B) / Bags (canal R) / Erros de combate (canal G)
	 // ------------------------------------------

	 // RECURSO PRIMÁRIO
	 int v_recurso = pixels[1].b & 127;                   // bits 0–6 → recurso proporcional (0–127)
	 int v_mana = (v_recurso * 100) / 127;                // convertido em porcentagem
	 e.mana = v_mana;                                     // atualiza atributo de mana
	 tb_mana.Text = v_mana.ToString();                    // exibe no textbox (debug)

	 // WAND: 
	 me.wand_up = (pixels[1].r & 8) != 0;                 // bit 3 = wand disponível (Shoot pronto e em range)
	 me.wandon = (pixels[1].b & 128) != 0;                // bit 7 = wand ativa
	 flags.wandGCD = (pixels[1].r & 0xF0) >> 4;           // bits 4-7 = wand GCD (0 a 15)

	 // SLOTS LIVRES NAS BAGS
	 me.freeslots = pixels[1].r & 7;                      // 3 bits, máximo 7 slots livres (bits 0-2)

	 // ERROS DE COMBATE (bitmask no canal G)
	 int g_erro = pixels[1].g;                            // canal G codifica erros combinados
	 me.wrongway = (g_erro & 128) != 0;                   // 128 = "You are facing the wrong way!"
	 me.outofrange = (g_erro & 64) != 0;                  //  64 = "Out of range" ou "You are too far away!"

	 if (me.wrongway)
	 {
		loga($"Wrong way!");
	 }

	 if (false && me.outofrange)
		loga($"Out of range!");

	 // ------------------------------------------
	 // Pixel 2 e 3 (Coordenadas X e Y)
	 // ------------------------------------------

	 // X
	 int dez_x = (int)Math.Round(pixels[2].r / 25.0);
	 int uni_x = (int)Math.Round(pixels[2].b / 25.0);
	 int dec_x = (int)Math.Round(pixels[2].g / 2.5);
	 int final_x = (dez_x * 10 + uni_x) * 100 + dec_x;
	 e.pos.x = final_x;
	 tb_x.Text = final_x.ToString(); // debug opcional

	 // Y
	 int dez_y = (int)Math.Round(pixels[3].r / 25.0);
	 int uni_y = (int)Math.Round(pixels[3].b / 25.0);
	 int dec_y = (int)Math.Round(pixels[3].g / 2.5);
	 int final_y = (dez_y * 10 + uni_y) * 100 + dec_y;
	 e.pos.y = final_y;
	 tb_y.Text = final_y.ToString(); // debug opcional


	 // ------------------------------------------
	 // PIXEL 4 - facing (yaw) em W e velocidade (spd)
	 // ------------------------------------------
	 if (pixels.Count > 4) // verifica se o pixel 4 existe
	 {
		// yaw (em W)
		double yaw_raw = pixels[4].r * 360.0 / 256.0; // converte de byte para grau real
		e.facing = (int)Math.Round(yaw_raw);         // converte para W (milésimos de pi-rad)
		if (true) tb_yaw.Text = e.facing.ToString();

		// velocidade
		e.spd = 255 - pixels[4].g; // canal G invertido
		if (true) tb_spd.Text = e.spd.ToString();

		// leitura do canal B
		var b4 = pixels[4].b;
		int sform_cd = b4 & 63;                     // bits 0–5 = cooldown da Stoneform (0–63)
		bool racial_up = (b4 & 64) != 0;             // bit 6 = racial disponível
																								 // bool furbolg = (b4 & 128) != 0;           // bit 7 = buff Furbolg ativo (REMOVIDO)

		if (cb_dwarf.Checked)
		 e.racialready = racial_up;

		// e.furbolg_form = cb_furbolg.Checked && furbolg; // REMOVIDO: Furbolg Form
	 }


	 // ----------------------------------------------
	 // PIXEL 5 - Combate + Barra de Respiração + CAST TARGET (NOVO) + Debuffs
	 // ----------------------------------------------
	 if (pixels.Count > 5)
	 {
		// R: Combate e Respiração
		e.combat = (pixels[5].r & 128) != 0; // está em combate
		e.swim = (pixels[5].r & 64) != 0;   // está nadando/barra de respiração ativa
		cb_combat.Checked = e.combat; // atualiza checkbox na UI

		// G: NOVO: Progresso da barra de cast do Target
		tar.castbar = (pixels[5].g * 100) / 255;
		tar.casting = tar.castbar > 0; // 'tar.casting' agora é derivado de 'tar.castbar'
																	 // tar.spell = ((char)pixels[11].b).ToString(); // REMOVIDO: Leitura de nome da spell
																	 // tar.spellid = 0; // REMOVIDO/Não necessário aqui

		tb_tarcast.Text = tar.casting ? "Casting..." : "-"; // Mostra status simples, não a letra
		pb_tarcast.Value = tar.castbar;

		// B: Tipos de debuffs
		int debuff_raw = pixels[5].b / 8; // extrai valor base dos debuffs (0–31)
		e.hascurse = (debuff_raw & 1) != 0;   // bit 0 → Curse
		e.hasother = (debuff_raw & 2) != 0;   // bit 1 → outro debuff (sem dispelType)
		e.haspoison = (debuff_raw & 4) != 0;  // bit 2 → Poison
		e.hasdisease = (debuff_raw & 8) != 0; // bit 3 → Disease
		e.hasmagic = (debuff_raw & 16) != 0;  // bit 4 → Magic
	 }

	 // -------------------------------------
	 // PIXEL 6: Player Level, Classe, CAST PLAYER (NOVO)
	 // -------------------------------------
	 if (pixels.Count > 6)
	 {
		// LEVEL
		e.level = (int)Math.Round(pixels[6].r / 4.0);
		tb_level.Text = e.level.ToString();

		// CLASSE
		// Nota: O valor do pixel 6.g já é a ID da classe do WoW Classic que o addon Lua envia.
		// O `isPaladino` era uma lógica interna do seu C#, mas podemos usar o valor direto.
		int class_id_raw = pixels[6].g;
		string className = "Unknown";
		if (class_id_raw == PALADIN) className = "Paladin";
		else if (class_id_raw == WARRIOR) className = "Warrior";
		else if (class_id_raw == HUNTER) className = "Hunter";
		else if (class_id_raw == ROGUE) className = "Rogue";
		else if (class_id_raw == PRIEST) className = "Priest";
		else if (class_id_raw == SHAMAN) className = "Shaman";
		else if (class_id_raw == MAGE) className = "Mage";
		else if (class_id_raw == WARLOCK) className = "Warlock";
		else if (class_id_raw == DRUID) className = "Druid";

		tb_class.Text = className;
		e.classe = class_id_raw; // Armazena o valor bruto da classe, se preferir

		me.iscaster = (e.classe == WARLOCK || e.classe == MAGE || e.classe == PRIEST);


		//loga(e.classe.ToString());

		// B: NOVO: Progresso da barra de cast do Player
		e.castbar = (pixels[6].b * 100) / 255;
		e.casting = e.castbar > 0; // 'e.casting' agora é derivado de 'e.castbar'
															 // e.spell = ((char)pixels[10].b).ToString(); // REMOVIDO: Leitura de nome da spell
															 // e.spellid = 0; // REMOVIDO/Não necessário aqui
    me.ready = e.casting; // Atualiza o estado de "pronto" do player 

		pb_playercast.Value = e.castbar; // Atualiza progressbar (0-100)
	 }


	 // -------------------------------------
	 // Pixel 7: Target HP, Flags, Level
	 // -------------------------------------
	 if (pixels.Count > 7)
	 {
		tar.hp = (pixels[7].r * 100) / 255;          // R: HP do alvo (%)
		tar.morreu = tar.hp == 0;                    // morreu se HP = 0
		tb_tarhp.Text = tar.hp.ToString();           // mostra na textbox

		e.hastarget = (pixels[7].g & 128) > 0;       // G: bit 7 → existe target
		tar.israre = (pixels[7].g & 64) > 0;         // G: bit 6 → raro
		tar.iselite = (pixels[7].g & 32) > 0;        // G: bit 5 → elite

		tar.level = (int)Math.Round(pixels[7].b / 4.0); // B: level do target (×4)
		tb_tarlevel.Text = tar.level.ToString();     // mostra na textbox
	 }

	 // -------------------------------------
	 // NOVO PIXEL 8: TIPO DA CRIATURA DO TARGET (Movido do antigo Pixel 14)
	 // -------------------------------------
	 if (pixels.Count > 8)
	 {
		tar.type = pixels[8].r; // lê valor bruto do canal R
		tb_tartype.Text = tar.type.ToString(); // exibe no textbox
	 }


	 // ------------------------------------------
	 // PIXEL 9 – AGGRO FLAGS + MOOD (2 BITS)
	 // ------------------------------------------
	 // R: bit 0 = player_aggro (mob atacando player)
	 //    bit 1 = pet_aggro (mob atacando pet)
	 //    bits 2-3 = mood (2 bits):
	 //               00 (0)  = neutro (mood = 0)
	 //               01 (4)  = amigável (mood = 1)
	 //               10 (8)  = hostil (mood = -1)
	 //               11 (12) = LIVRE
	 //    bits 4-7 = LIVRES
	 // G: livre
	 // B: livre
	 // ------------------------------------------

	 if (pixels.Count > 9)
	 {
		int r9 = pixels[9].r; // canal vermelho com as flags

		// Lê as flags booleanas de aggro
		tar.player_aggro = (r9 & 1) != 0;    // bit 0 = player tem aggro
		tar.pet_aggro = (r9 & 2) != 0;      // bit 1 = pet tem aggro

		// Lê o mood do target (2 bits: 2-3)
		int mood_bits = (r9 & 12) >> 2;     // extrai bits 2-3 e desloca para posição 0-1

		// Decodifica mood baseado nos 2 bits
		switch (mood_bits)
		{
		 case 0: tar.mood = 0; break;    // 00 = neutro
		 case 1: tar.mood = 1; break;    // 01 = amigável 
		 case 2: tar.mood = -1; break;    // 10 = hostil
		 case 3: tar.mood = 0; break;    // 11 = reservado/padrão neutro
		}
	 }

	 // DEFINIÇAO DE TRIVIAL - NAO APAGAR AO EDITAR O PIXEL 9
	 tar.trivial = !tar.iselite && (tar.hp <= 25 || tar.level <= me.level - 3);

	 if (e.classe == WARRIOR) // se for Warrior, lê o Pixel 10
	 {
		int r10 = pixels[10].r; // canal vermelho
		int g10 = pixels[10].g; // canal verde
		int b10 = pixels[10].b; // canal azul

		war.charge_up = (r10 & 1) != 0;      // bit 0 = pode usar Charge?
		war.throw_up = (r10 & 2) != 0;      // bit 1 = pode usar ranged (throw)
		war.bs_up = (r10 & 4) != 0;      // bit 2 = pode usar Battle Shout?
		war.cleave_up = (r10 & 8) != 0;      // bit 3 = pode usar Cleave?
		war.demo_up = (r10 & 16) != 0;     // bit 4 = pode usar Demoralizing Shout?
		war.rend_up = (r10 & 32) != 0;     // bit 5 = pode usar Rend?
		war.thun_up = (r10 & 64) != 0;     // bit 6 = pode usar Thunder Clap?
		war.hs_up = (r10 & 128) != 0;    // bit 7 = pode usar Heroic Strike?

		war.has_cleave = (g10 & 1) != 0;      // bit 0 = Cleave toggle ativo
		war.overpower_up = (g10 & 2) != 0;      // bit 1 = Overpower pronto
		war.bloodrage_up = (g10 & 4) != 0;      // bit 2 = Bloodrage pronto
		war.hams_up = (g10 & 8) != 0;      // bit 3 = Hamstring pronto
		war.retaliation_up = (g10 & 16) != 0;     // bit 4 = Retaliation pronto
		war.dwish_up = (g10 & 32) != 0;     // bit 5 = Death Wish pronto
		war.has_bs = (g10 & 64) != 0;     // bit 6 = Battle Shout buff ativo
		war.has_rend = (g10 & 128) != 0;    // bit 7 = target tem Rend

		war.hs_casting = (b10 & 1) != 0;      // bit 0 = Heroic Strike toggle ativo
		war.slam_up = (b10 & 2) != 0;      // bit 1 = Slam pronto e em range
		war.execute_up = (b10 & 4) != 0;      // bit 2 = Execute pronto
		war.has_demoralizing = (b10 & 8) != 0;      // bit 3 = target com Demoralizing Shout
		war.sunder_up = (b10 & 16) != 0;     // bit 4 = Sunder Armor pronto e stacks < 5
	 }




	 // -------------------------------------------
	 // PIXEL 10 – WARLOCK
	 // -------------------------------------------
	 // R: bit 0     = immolate_up             (spell pronta + range + mana)
	 // R: bit 1     = corruption_up           (spell pronta + range + mana)
	 // R: bit 2     = curse_weakness_up       (spell pronta + range + mana)
	 // R: bit 3     = curse_agony_up          (spell pronta + range + mana)
	 // R: bit 4     = drain_soul_up           (spell pronta + range + mana)
	 // R: bit 5     = drain_life_up           (spell pronta + range + mana)
	 // R: bit 6     = siphon_life_up          (spell pronta + range + mana)
	 // R: bit 7     = shadowbolt_up           (spell pronta + range + mana)
	 //
	 // G: bit 0     = has_immolate            (>1 seg restante no target)
	 // G: bit 1     = has_corruption          (>1 seg restante no target)
	 // G: bit 2     = has_curse_weakness      (ativo no target)
	 // G: bit 3     = has_curse_agony         (ativo no target)
	 // G: bit 4     = has_drain_soul          (channeling ativo)
	 // G: bit 5     = has_drain_life          (channeling ativo)
	 // G: bit 6     = has_siphon_life         (ativo no target)
	 // G: bit 7     = healthstone usable      (Healthstone na bag e cooldown == 0)
	 //
	 // B: bit 0     = has_demon_skin/armor    (aura ativa)
	 // B: bit 1     = has_pet                 (pet vivo e ativo)
	 // B: bit 2     = life_tap_up             (spell pronta + range + mana)
	 // B: bits 3-4  = número de Soul Shards   (0..3 shards)
	 // B: bit 5     = has_healthstone         (tem Healthstone na bag)
	 // B: bit 6     = create_healthstone_up   (spell up + shard ≥ 1)
	 // B: bit 7     = health funnel up        (spell pronta + range + mana)
	 // -------------------------------------------
	 //
	 // PIXEL 11 – WARLOCK
	 // -------------------------------------------
	 // R: bits 0-6  = pet.hp % (proporcional base 127 → 0..127)
	 // R: bit 7     = LIVRE
	 // G: LIVRE
	 // B: LIVRE
	 // -------------------------------------------


	 // -------------------------------------------

	 // -------------------------------------------
	 // PIXEL 10 (Warlock) – SPELLS, DEBUFFS, PET STATUS
	 // -------------------------------------------
	 else if (me.classe == WARLOCK)
	 {
		int ar10 = pixels[10].r;  // R = spells prontas (cooldown + range + mana)
		int ag10 = pixels[10].g;  // G = debuffs no target
		int ab10 = pixels[10].b;  // B = pet status

		wlock.immolate_up = (ar10 & 1) != 0;
		wlock.corruption_up = (ar10 & 2) != 0;
		wlock.curse_weakness_up = (ar10 & 4) != 0;
		wlock.curse_agony_up = (ar10 & 8) != 0;
		wlock.drain_soul_up = (ar10 & 16) != 0;
		wlock.drain_life_up = (ar10 & 32) != 0;
		wlock.siphon_life_up = (ar10 & 64) != 0;
		wlock.shadowbolt_up = (ar10 & 128) != 0;

		wlock.has_immolate = (ag10 & 1) != 0;
		wlock.has_corruption = (ag10 & 2) != 0;
		wlock.has_curse_weakness = (ag10 & 4) != 0;
		wlock.has_curse_agony = (ag10 & 8) != 0;
		wlock.has_drain_soul = (ag10 & 16) != 0;
		wlock.has_drain_life = (ag10 & 32) != 0;
		wlock.has_siphon_life = (ag10 & 64) != 0;
		wlock.healthstone_up = (ag10 & 128) != 0;   // G7 → usable

		wlock.has_demon_skin = (ab10 & 1) != 0;
		wlock.has_pet = (ab10 & 2) != 0;
		wlock.lifetap_up = (ab10 & 4) != 0;
		wlock.shards = (ab10 >> 3) & 0b11;  // bits 3-4
		wlock.has_healthstone = (ab10 & 32) != 0;
		wlock.create_healthstone_up = (ab10 & 64) != 0;
		wlock.healhfunnel_up = (ab10 & 128) != 0;

		// -------------------------------------------
		// PIXEL 11 (Warlock) – PET HP (%)
		// -------------------------------------------
		int ar11 = pixels[11].r;   // R = pet.hp % (base 127 → 0..127)

		wlock.pet_hp = (int)(ar11 * 100.0 / 127.0);   // pet.hp % (0..100)
	 }

	 // =====================================
	 // PIXEL 10: HUNTER STATUS (VERSÃO EXPANDIDA)
	 // Substituir a seção existente do Hunter no método getstats
	 // =====================================
	 else if (me.classe == HUNTER)
	 {
		// ----------------------------------------
		// HUNTER - PIXEL 10 EXPANDIDO
		// ----------------------------------------
		// PIXEL 10 (Hunter) – Skills prontas + Status ativo + Debuffs
		// ----------------------------------------
		// R: bit 0 = raptor_strike_up      (Raptor Strike pronto - mana + range)
		//    bit 1 = auto_shot_up          (Auto Shot pronto e em range)
		//    bit 2 = auto_shot_range_ok    (Auto Shot range ok - verificação específica)
		//    bit 3 = serpent_sting_up      (NOVO: Serpent Sting pronto)
		//    bit 4 = concussive_shot_up    (NOVO: Concussive Shot pronto)
		//    bit 5 = arcane_shot_up        (NOVO: Arcane Shot pronto)
		//    bit 6 = revive_pet_up         (NOVO: Revive Pet pronto)
		//    bit 7 = LIVRE

		// G: bit 0 = auto_shot_ativo       (Auto Shot ativo - como autoattack)
		//    bit 1 = tar_serpent           (NOVO: Target tem debuff Serpent Sting)
		//    bits 2-7 = LIVRES

		// B: bit 0 = raptor_strike_toggle_ativo (Raptor Strike toggle ativo)
		//    bit 1 = has_pet               (Pet vivo e ativo)
		//    bits 2-7 = LIVRES

		int r10 = pixels[10].r; // canal vermelho
		int g10 = pixels[10].g; // canal verde  
		int b10 = pixels[10].b; // canal azul

		// ================================
		// CANAL R: Skills prontas
		// ================================
		hunt.raptor_strike_up = (r10 & 1) != 0;        // bit 0 = Raptor Strike pronto (mana + range + toggle nao ativo)
		hunt.auto_shot_up = (r10 & 2) != 0;            // bit 1 = Auto Shot pronto e em range
		hunt.auto_shot_range_ok = (r10 & 4) != 0;      // bit 2 = Auto Shot range ok

		// NOVAS SKILLS
		hunt.serpent_sting_up = (r10 & 8) != 0;        // bit 3 = Serpent Sting pronto
		hunt.concussive_shot_up = (r10 & 16) != 0;     // bit 4 = Concussive Shot pronto
		hunt.arcane_shot_up = (r10 & 32) != 0;         // bit 5 = Arcane Shot pronto
		hunt.revive_pet_up = (r10 & 64) != 0;          // bit 6 = Revive Pet pronto

		// ================================
		// CANAL G: Status ativo e debuffs
		// ================================
		hunt.auto_shot_ativo = (g10 & 1) != 0;         // bit 0 = Auto Shot ativo (como autoattack)
		hunt.tar_serpent = (g10 & 2) != 0;             // bit 1 = Target tem debuff Serpent Sting

		// ================================
		// CANAL B: Toggle ativo e pet status
		// ================================
		hunt.has_pet = (b10 & 2) != 0;                     // bit 1 = Pet vivo e ativo

		// ================================
		// PIXEL 11: PET HP (como Warlock)
		// ================================
		if (pixels.Count > 11)
		{
		 int ar11 = pixels[11].r;   // R = pet.hp % (base 127 → 0..127)
		 hunt.pet_hp = (int)(ar11 * 100.0 / 127.0);   // pet.hp % (0..100)
		}
	 }


	 // -------------------------------------
	 // PIXEL 10: PRIEST STATUS
	 // -------------------------------------
	 else if (me.classe == PRIEST)
	 {
		// CANAL R: Spells prontas
		priest.pws_up = (pixels[10].r & 1) != 0;
		priest.renew_up = (pixels[10].r & 2) != 0;
		priest.fortitude_up = (pixels[10].r & 4) != 0;
		priest.innerfire_up = (pixels[10].r & 8) != 0;
		priest.innerfocus_up = (pixels[10].r & 16) != 0;
		priest.swp_up = (pixels[10].r & 32) != 0;
		priest.smite_up = (pixels[10].r & 64) != 0;        // NOVO
		priest.heal_up = (pixels[10].r & 128) != 0;        // NOVO

		// CANAL G: Buffs e debuffs
		priest.has_pws = (pixels[10].g & 1) != 0;
		priest.has_renew = (pixels[10].g & 2) != 0;
		priest.has_fortitude = (pixels[10].g & 4) != 0;
		priest.has_innerfire = (pixels[10].g & 8) != 0;
		priest.has_innerfocus = (pixels[10].g & 16) != 0;
		priest.has_swp = (pixels[10].g & 32) != 0;
		priest.has_weakened_soul = (pixels[10].g & 64) != 0;
		priest.has_divine_spirit = (pixels[10].g & 128) != 0;

		// CANAL B: Cooldowns defensivos
		priest.desperate_prayer_up = (pixels[10].b & 1) != 0;  // Desperate Prayer pronto
	 }


	 else if (e.classe == ROGUE) // se for Rogue, lê o Pixel 10
	 {
		// ----------------------------------------
		// ROGUE
		// ----------------------------------------
		// PIXEL 10 (Rogue) – Combo Points + CDs + Auras + Debuffs
		// ----------------------------------------
		// R: bits 0–2 = número de combo points (0 a 7)
		//     bit 0 = +1
		//     bit 1 = +2
		//     bit 2 = +4
		//     bit 3 = throw_up         (Throw ou Shoot pronto e em range)
		//     bit 4 = stealth_up       (pode ativar Stealth agora)
		//     bit 5 = evasion_up       (Evasion pronto)
		//     bit 6 = sad_up           (Slice and Dice pronto)
		//     bit 7 = expose_up        (Expose Armor pronto + energia ≥ 25)

		// G: bit 0 = has_stealth       (aura de Stealth ativa)
		//     bit 1 = has_sad          (aura Slice and Dice ativa)
		//     bit 2 = kick_up          (Kick pronto e em range)

		// B: bit 0 = has_expose        (target com debuff Expose Armor)
		//     bit 1 = vanish_up        (Vanish pronto)
		//     bit 2 = riposte_up       (Riposte pronto)
		//     bit 3 = gouge_up         (Gouge pronto)
		//     bit 4 = rupture_up       (Rupture pronto + energia suficiente)
		//     bit 5 = has_rupture      (target com debuff Rupture)

		int r10 = pixels[10].r;
		int g10 = pixels[10].g;
		int b10 = pixels[10].b;

		// ----------------------------------------
		// RED (R) – combo, CDs e habilidades
		// ----------------------------------------
		rog.combo = r10 & 7;                           // bits 0–2 = combo points
		rog.throw_up = (r10 & 8) != 0;                 // bit 3 = throw_up
		rog.stealth_up = (r10 & 16) != 0;              // bit 4 = stealth_up
		rog.evasion_up = (r10 & 32) != 0;              // bit 5 = evasion_up
		rog.SAD_up = (r10 & 64) != 0;                  // bit 6 = Slice and Dice pode ser usado
		rog.expose_armor_up = (r10 & 128) != 0;        // bit 7 = Expose Armor pode ser usado

		// ----------------------------------------
		// GREEN (G) – auras e kick
		// ----------------------------------------
		rog.stealth = (g10 & 1) != 0;                  // bit 0 = aura Stealth ativa
		rog.has_SAD = (g10 & 2) != 0;                  // bit 1 = aura Slice and Dice ativa
		rog.kick_up = me.melee && (g10 & 4) != 0;      // bit 2 = Kick pronto e em range

		// ----------------------------------------
		// BLUE (B) – debuffs no target e CDs adicionais
		// ----------------------------------------
		rog.has_expose_armor = (b10 & 1) != 0;         // bit 0 = target com debuff Expose Armor
		rog.vanish_up = (b10 & 2) != 0;                // bit 1 = Vanish pronto
		rog.riposte_up = (b10 & 4) != 0;               // bit 2 = Riposte pronto
		rog.gouge_up = (b10 & 8) != 0;                 // bit 3 = Gouge pronto
		rog.rupture_up = (b10 & 16) != 0;              // bit 4 = Rupture pronto
		rog.has_rupture = (b10 & 32) != 0;             // bit 5 = target com debuff Rupture

		// ----------------------------------------
		// Derivados por condição atual
		// ----------------------------------------
		tb_combos.Text = rog.combo.ToString();         // exibe combo points na tela

		rog.evis_up = me.mana > 35 && rog.combo > 0 && me.melee; // Eviscerate disponível
		rog.ss_up = me.mana > 45 && me.melee;                  // Sinister Strike disponível

	 }

	 // ====================================================================
	 // PIXELS ESPECÍFICOS DE PALADINO (10 a 15)
	 // ====================================================================

	 // Condicional para classe Paladino
	 else if (e.classe == PALADIN) // Usa o valor 255 para Paladino que o Lua envia
	 {
		// -------------------------------------
		// NOVO PIXEL 10: Judgement (alcance, cooldown) + Auras (Movido do antigo Pixel 9)
		// -------------------------------------
		if (pixels.Count > 10)
		{
		 // canal R → está em alcance
		 pala.jud_range = (pixels[10].r > 250); // Judgement em alcance

		 // canal G → cooldown restante (0–255)
		 pala.judge_cd = pixels[10].g; // cooldown em segundos (cap 255)

		 // canal B → decodifica bitflags de aura
		 int b10 = pixels[10].b; // lê valor bruto (0–255)
														 // e.meleerange = (b10 & 1) != 0; // REMOVIDO: movido para Pixel 0
		 pala.crusader = (b10 & 2) != 0; // bit 1 = Crusader Aura
		 pala.devotion = (b10 & 4) != 0; // bit 2 = Devotion Aura
		 pala.frost = (b10 & 8) != 0; // bit 3 = Frost Resist Aura
		 pala.shadow = (b10 & 16) != 0; // bit 4 = Shadow Resist Aura
		 pala.fire = (b10 & 32) != 0; // bit 5 = Fire Resist Aura
		 pala.concentration = (b10 & 64) != 0; // bit 6 = Concentration Aura
		 pala.retribution = (b10 & 128) != 0; // bit 7 = Retribution Aura
		}


		// -------------------------------------
		// NOVO PIXEL 11: VAZIO (Conteúdo Cast Player/Target movido para Pixel 5 e 6)
		// -------------------------------------
		// Não há lógica de leitura, pois o pixel estará zerado/vazio
		// Certifique-se de que suas variáveis C# correspondentes (se existirem) estejam zeradas/falsas por padrão.

		// -------------------------------------
		// NOVO PIXEL 12: VAZIO (Conteúdo Cast Target movido para Pixel 5)
		// -------------------------------------
		// Não há lógica de leitura, pois o pixel estará zerado/vazio
		// Certifique-se de que suas variáveis C# correspondentes (se existirem) estejam zeradas/falsas por padrão.


		// -------------------------------------
		// NOVO PIXEL 13: Seals ativos (R) + Judgements no target (G) + Cooldowns Defensivos (B) (Movido do antigo Pixel 12)
		// -------------------------------------
		if (pixels.Count > 13)
		{
		 int ar13 = pixels[13].r; // R = bitmask dos seals ativos
		 int ag13 = pixels[13].g; // G = bitmask dos judgements no target
		 int ab13 = pixels[13].b; // B = bitflags de cooldowns defensivos

		 // SEALS ATIVOS (true se bit correspondente estiver ligado)
		 pala.sor = (ar13 & 128) != 0; // Seal of Righteousness
		 pala.sotc = (ar13 & 64) != 0; // Seal of the Crusader
		 pala.soc = (ar13 & 4) != 0; // Seal of Command
		 pala.sol = (ar13 & 16) != 0; // Seal of Light
		 pala.sow = (ar13 & 8) != 0; // Seal of Wisdom
		 pala.soj = (ar13 & 32) != 0; // Seal of Justice (Adicionado: estava faltando na lista do C#)

		 // JUDGEMENTS NO TARGET
		 pala.jotc = (ag13 & 64) != 0; // Judgement of the Crusader
		 pala.joj = (ag13 & 32) != 0; // Judgement of Justice
		 pala.jol = (ag13 & 16) != 0; // Judgement of Light
		 pala.jow = (ag13 & 8) != 0; // Judgement of Wisdom

		 // FLAGS (CANCAST / DEBUFF)
		 pala.cancast_LOH = (ab13 & 128) != 0; // pode castar Lay on Hands
		 pala.BOP_up = (ab13 & 64) != 0; // pode castar Blessing of Protection
		 pala.divine_shield_up = (ab13 & 32) != 0; // Divine Shield (movido)

		 // NOVO: Divine Protection Ready (movido do antigo Pixel 6)
		 pala.divine_protection_up = (ab13 & 16) != 0;

		 pala.forbearance = (ag13 & 128) != 0; // tem debuff Forbearance

		 // Anula os cooldowns defensivos se estiver sob Forbearance
		 if (pala.forbearance)
		 {
			pala.divine_protection_up = false;
			pala.divine_shield_up = false;
			pala.BOP_up = false; // Anula BOP se Forbearance ativo
		 }
		}


		// -------------------------------------
		// NOVO PIXEL 14: Blessings ativos (R) + HoJ ready (G bit 7) + HoJ range (B) (Movido do antigo Pixel 13)
		// -------------------------------------
		if (pixels.Count > 14)
		{
		 int ar14 = pixels[14].r; // R = bitmask dos blessings ativos
		 int ag14 = pixels[14].g; // G = bitflags de cooldowns (bit 7 = HoJ pronto)
		 int ab14 = pixels[14].b; // B = 255 se HoJ está em alcance

		 // BLESSINGS ATIVOS (bit por ordem nova)
		 pala.bos = (ar14 & 1) != 0; // bit 0 = Salvation
		 pala.bol = (ar14 & 2) != 0; // bit 1 = Light
		 pala.bof = (ar14 & 4) != 0; // bit 2 = Freedom
		 pala.bow = (ar14 & 8) != 0; // bit 3 = Wisdom
		 pala.bop = (ar14 & 16) != 0; // bit 4 = Protection
		 pala.bok = (ar14 & 32) != 0; // bit 5 = Kings
		 pala.bom = (ar14 & 64) != 0; // bit 6 = Might
		 pala.bosanc = (ar14 & 128) != 0; // bit 7 = Sanctuary

		 // HAMMER OF JUSTICE
		 pala.hoj_ready = (ag14 & 128) != 0; // está pronto
		 pala.hoj_range = (ab14 > 250);      // está em alcance
		 cb_hammer_range.Checked = pala.hoj_range;
		}

		// -------------------------------------
		// NOVO PIXEL 15: VAZIO (Específico de Paladino - mas não usado agora)
		// -------------------------------------
		// Se a classe é Paladino, mas este pixel não é usado, garanta que seja preto
		// Não há lógica de leitura, pois o pixel estará zerado/vazio
		// Certifique-se de que suas variáveis C# correspondentes (se existirem) estejam zeradas/falsas por padrão.
		// Já estará zerado pelo "else" abaixo se não for Paladino.
	 }
	 else // Se a classe NÃO for Paladino, zera TODOS os pixels de 10 a 15
	 {

	 }
	}

	// M03 - MÉTODO WAIT - ESPERA SEM TRAVAR A JANELA.
	public void wait(int milliseconds)
{
System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew(); // Inicia cronômetro para medir tempo
while (sw.ElapsedMilliseconds <= milliseconds) // Continua até atingir o tempo especificado
{
 Application.DoEvents(); // Processa eventos da interface para evitar travamento
 System.Threading.Thread.Sleep(1); // Pausa breve para reduzir uso de CPU
}
}




// M05 - MÉTODO APERTA - ENVIA UM PRESSIONAMENTO DE TECLA PARA O WOW.
public void aperta(byte key, int time = 40) // time 0 = pressiona, time 2 = solta
{
	 if (key == 255) return; // ignora teclas nulas (usamos 255 como código de "nada")
	 focawow(); // Garante que a janela do WoW está em foco
	 if (time == 2) // codigo de soltar a tecla
	 {
		keybd_event(key, 0, KEYEVENTF_KEYUP, 0); // Simula soltura da tecla
		return;
	 }
	 keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0); // Simula pressionamento da tecla
	 if (time > 0)
	 { 
		wait(time);
		keybd_event(key, 0, KEYEVENTF_KEYUP, 0); // Simula soltura da tecla
	 }
}
// MINI METODOS AUXILIARES DE APERTA: PRESS e SOLTA 
void press(byte key)
	{
	 keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0); // Simula pressionamento da tecla
	}
	// MINI METODOS AUXILIARES DE APERTA: SOLTA
	void solta(byte key)
	{
	 keybd_event(key, 0, KEYEVENTF_KEYUP, 0); // Simula soltura da tecla
	}
	



	// M01 - MÉTODO CLICA - MOVE O MOUSE PARA (loc.x, loc.y) E CLICA COM O BOTÃO (padrão: direito)
	public void clica(loc p, int botao = 2)
	{
	 mousemove(p.x, p.y);     // move até o ponto
	 DoMouseClick(botao);     // executa clique com botão escolhido
	}


	bool permitido_giro = false; // variável para controlar se pode girar ou não
													// --------------------------------
													// MÉTODO GIRALVO 5.2 - GIRA O PERSONAGEM PARA A COORDENADA ALVO (versão 0–360)
													// --------------------------------
	public void giralvo(loc alvo)
	{
	 giraface(getyaw(me.pos, alvo), dist(me.pos, alvo));
	 permitido_giro = podegirar(alvo); // atualiza se pode girar ou não
	}


	// -----------------------------------------------------------
	// M11 - MÉTODO DIST - CALCULA A DISTÂNCIA ENTRE DUAS COORDENADAS (LOC)
	// Corrige o eixo Y comprimido dividindo por 1.515 se modo humanlike estiver ativo
	// -----------------------------------------------------------
	public int dist(loc orig, loc tar)
	{
	 double dx = orig.x - tar.x;                  // diferença no eixo X
	 double dy = orig.y - tar.y;                  // diferença no eixo Y

	 if (true || cb_humanlike.Checked)                    // se modo humanlike ativado
		dy /= 1.515;                             // divide pra corrigir compressão do Y

	 double distance = Math.Sqrt(dx * dx + dy * dy); // pitágoras
	 return (int)distance;                        // retorna como inteiro
	}



	//-----------------------------------------------------------
	// MÓDULO 18 - VARIÁVEL GLOBAL -VARIÁVEIS GLOBAIS 
	//--------------------------------------------------------


	// --------------------------------
	// INSTÂNCIAS DAS CLASSES DE STATUS
	// --------------------------------
	public element me;
	public element tar;
	flag flags = new flag(); // flags gerais do jogo 
	palatable pala = new palatable();
	roguetable rog = new roguetable();
	warriortable war = new warriortable();
	locktable wlock = new locktable();  // NOVA INSTÂNCIA WARLOCK
	huntertable hunt = new huntertable(); // Instancia do hunter 

	public static priestable priest = new priestable();




	public HashSet<loc> hash_planta = new HashSet<loc>(); // inicializa tabela de plantas encontradas

	// =====================================================
	// SISTEMA DE OTIMIZAÇÃO ADAPTATIVA PARA COMBO POINTS
	// =====================================================

	// Variáveis globais para o sistema de otimização
	



	//------------------------------
	// NAO DEIXA AFOGAR 
	//------------------------------
	void nao_afoga()
	{
	 if (me.swim)
	 {
		aperta(PULA, 2000); // nada para cima se estiver afogando
		loga("Afogando: Nadando para cima.");
	 }
	}
	// --------------------------------
	// MÓDULO 14 - MÉTODO MOVETO (MAIN LOOP)
	// Anda até destino, corrige direção, reage a combate, detecta morte e unstuck
	// --------------------------------
	public void moveto(loc destino)
	{
	 // --------------------------------
	 // ROTINAS GERAIS PRÉ-MOVIMENTAÇÃO
	 // --------------------------------
	 reset_stuck_system(); // inicializa o sistema anti stuck 
	 Func<bool> has_seal = () => pala.sor || pala.soc || pala.sow || pala.sol || pala.sotc; // tem algum seal ativo

	 Func<int, bool> mana = (p) => me.mana > p;            // verifica se tem mana acima de p

	 getstats(ref me);                                     // atualiza status inicial
	 drawmap(destino);
	 if (cb_herbalism.Checked) atualizamapa(me.pos);
	 int temp = 0;                                         // contador de ciclos
	 loc oldloc = me.pos;                                  // guarda posição inicial para unstuck
	 Stopwatch timeout = new Stopwatch();                  // cronômetro para evitar travamento
	 timeout.Start();                                      // inicia contagem

	 //-----------------------------------------
	 // UNSTUCK GERAL:
	 //-----------------------------------------
	 // Mais de 5 minutos sem matar nada 
	 if (Environment.TickCount - last_kill_time > 300000) // 5 min sem kill
	 {
		loga("Inatividade de 5 minutos. Usando Hearthstone.");
		HS();
	 }

	 //------------------------------
	 // DECISÃO DE ANDAR OU PARAR
	 //------------------------------
	 if (cb_anda.Checked)
	 {
		if (cb_humanlike.Checked) // modo fluido e menos robótico
		{
		 if (Math.Abs(delta(me.pos, destino)) < 160)
			press(WKEY); // curva possível → segue andando
		 else
			solta(WKEY); // curva fechada → para
		}
		else // modo clássico com base no delta angular
		{
		 if (dist(me.pos, destino) > 100 || Math.Abs(delta(me.pos, destino)) < 150)
			press(WKEY); // pode andar
		 else
		 {
			solta(WKEY); // ângulo muito grande → para
			aperta(PULA); // pula para corrigir posição PULANDO
		 }
		}
	 }
	 else
	 {
		solta(WKEY); // segurança: desativa andar se cb_anda não estiver marcado
	 }



	 long last = 0; // marca o tempo do último evento em milissegundos
	 ///------------------------------------------------------
	 //                 COMEÇA A ANDAR  
	 //--------------------------------------------------------
	 do
	 {
		drawmap(destino); // atualiza o mapa com o destino
											// --------------------------------
											// LOOP DE PAUSA
											// --------------------------------
		atualizamapa(me.pos);
		get_minimap(); // atualiza minimapa

		nao_afoga(); // se afogando, tenta nadar

		// ================================
		// CHAMADA DO SISTEMA ANTI-STUCK
		// ================================
		if (check_stuck() > 1) return; // ← SÓ ISSO!


		// -----------------------------
		// HERBALISTA - CATA PLANTA
		// -----------------------------
		loc planta = new loc(0, 0); // inicializa planta
		if (cb_herbalism.Checked)
		{
		 
		 planta = find_plants();
		 if (!catando_planta && planta.x != 0 && near_hash(planta, hash_planta) > 20)
		 {
			hash_planta.Add(planta);   // adiciona planta nova ao hash
			cataplanta(planta);        // vai atrás da planta
		 }

		}



		// -----------------------------
		// TIMEOUT DE 40 SEGUNDOS
		// -----------------------------
		if (timeout.ElapsedMilliseconds > 40000)
		{
		 solta(ANDA); // para de andar se timeout
		 loga("Timeout no moveto() — passando para o próximo ponto.");
		 return;
		}
		temp++;
		checkme(); // atualiza estado do personagem

		// ROGUE 
		// ainda não implementado, mas pode ser feito aqui
//------------------------		
		// -----------------------------
		// ANDAR (direção e pulo)
		// -----------------------------
		if (cb_anda.Checked)
		{
		 if (timeout.ElapsedMilliseconds - last >= 3000) // passaram 4 segundos?
		 {
			aperta(SPACEBAR); // dá pulinho a cada 4 segudnos
			last = timeout.ElapsedMilliseconds; // atualiza o tempo do último disparo
		 }
		 checkme();
		 drawmap(destino);
		 loga($"Distancia do waypoint: {dist(me.pos, destino)}");
		 //--------------------------------------------------------
		 // GIRA PARA O DESTINO (ALINHA FACING) 
		 //---------------------------------------------------------
		 int dif = delta(me.pos, destino);              // diferença angular com sinal
		 double d = dist(me.pos, destino);              // distância até o destino
		 double yaw = getyaw(me.pos, destino);          // direção até o destino

		 loga($"Yaw até o destino: {yaw}°");
		 loga($"Delta para virar: {Math.Abs(dif)}° ({(dif < 0 ? "esquerda" : "direita")})");
		 loga($"Distância até o destino: {d}");

		 /*
		 if (podegirar(destino))
			loga("Curva possível – seguindo em movimento.");
		 else
			loga("Curva muito fechada – será necessário parar.");
		 */
		 giralvo(destino);
		 // gira para o destino
		 //--------------------------------------------------------
		 drawmap(destino);
		 press(WKEY);
		}
		else
		 aperta(WKEY, 2); // solta W se não pode andar
											// -----------------------------
											// UNSTUCK e TARGET
											// -----------------------------
		if (true || temp % 2 == 0) // sempre
		{
		 checkme();

		 if (cb_anda.Checked && !me.combat && dist(me.pos, oldloc) <= 10)
		 {
			stuckcount++;
			//if (cb_log.Checked) loga("enrosco lvl " + stuckcount);
			if (stuckcount > 0)
			{
			// unstuck();       // executa pulo + giro se necessário
												//stuckcount = 0;  // reseta após tentativa
			}
		 }
		 else
		 {
			stuckcount = 0; // se está se movendo, zera contador
		 }


		 oldloc = me.pos; // atualiza posição ao final
		}

		// -----------------------------
		// BUFFS OUT OF COMBAT 
		// -----------------------------
		// PALADINO
		if (me.classe == PALADIN) bless(me); // 



		// -----------------------------
		// LIMPA TARGET MORTO FORA DE COMBATE
		// -----------------------------
		if (!me.combat && tar.morreu && me.hastarget)
		{
		 aperta(SETE); // limpa target se estiver morto
		 loga("Target morto fora de combate — limpando.");
		}

		// -----------------------------
		// PULL CODE (ataque inicial)
		// -----------------------------
		if (cb_anda.Checked && cb_pull.Checked && !me.combat)
		 puxa();

		// -----------------------------
		// COMBATE
		// -----------------------------
		if (me.combat)
		{
		 if (!me.iscaster) para(); // para de andar se em combate
		 clog("Entrou em combate!");
		 if (!on) return; // se desligado, sai do loop
		 loga("Chamando combatloop()...");
		 if (me.classe==ROGUE ) ComboOptimizer.categoria_atual = ComboOptimizer.EscolherCategoria(); // se for Rogue, escolhe categoria de combo points
		 combatloop(); // entra na rotina de combate
		 aperta(STOPATTACK); // para de atacar após combate

		 // -----------------------------------
		 // CHECK PÓS COMBATE 
		 // -----------------------------------
		 // SE PALADINO
		 //------------
		 // if paladino.... 
		 if (me.classe == PALADIN)
		 { 
		 if (me.hp < atoi(tb_preheal))
		 {
			if (mana(20) && (me.hp < 60 || !cb_flashheal.Checked))
			{
			 aperta(HLIGHT); wait(2500);
			 logacura("HOly Light", "HP > 60, MANA > 20, HP < 60 || Flashheal unckecked");
			}
			else
			 casta(FLASHHEAL);

			 bless(me);
		 }
		 checkme();
		 //----------------LIMPA POISONS-------------
		 if (me.level >= 8 // checa level
			 && cb_purify.Checked // checkbox ativo
			 && (me.haspoison || me.hasdisease) // tem debuff
			 && mana(30) // tem mana
			 )
			aperta(PURIFY, GCD); // ativa o purify
			 }
		 // -----------------------------------
		 // LOOT 
		 // -----------------------------------
		 
		 scan_elites(); // verifica se tem elite no mapa antes de clicar na tela 

		 if (cb_apagacinza.Checked)
		 {
			clica(new loc(5, 5), 1); // APAGA UM ITEM CINZA DA BAG, SE TIVER (botão criado pelo addon de apagar itens cinzas)
			wait(50); // espera 10ms para evitar spam
			clica(new loc(5, 5), 1); // APAGA UM ITEM CINZA DA BAG, SE TIVER (botão criado pelo addon de apagar itens cinzas)
		 
		 }
		 if (has(me.freeslots) || cb_loot_cloth.Checked) // só loota se estiver ativado, e se tiver espaço ou for loot de pano

		 {
			if (!me.combat)
			{
			 // --------------------------------
			 // ETAPA 5: LOOP DE LOOT OTIMIZADO
			 // Substituir o while(true) atual no sistema de loot
			 // --------------------------------
			 if (cb_loot.Checked)
			 {
				int loopcount = 0;
				int max_scans = calcula_max_scans(); // <<<< NOVA LÓGICA AQUI!
				primeira_tentativa_sessao = true;     // reseta flag para nova sessão
				ultimo_tipo_loot = 0;                 // limpa tipo anterior
				//loga("Nova sessão de loot iniciada.");



				while (loopcount < max_scans) // <<<< ERA >= 6, AGORA É DINÂMICO!
				{
				 checkme();                  // atualiza dados
				 if (me.combat) break;      // se entrou em combate, para tudo

				 loopcount++; // incrementa ANTES do scan

				 loc p = scanloot();        // tenta encontrar algo clicável
				 if (p.x < 0)
				 {
					loga($"Scan {loopcount}/{max_scans}: nada encontrado - finalizando loot.");
					break;        // nada encontrado, pode parar
				 }

				 loga($"Scan {loopcount}/{max_scans}: loot encontrado em ({p.x},{p.y})");
				 clica(p);                  // clica diretamente
				 checkme();

				 if (me.spd == 0 && !me.combat)
				 {
					wait(400);            // espera loot ou skin
					if (cb_skinning.Checked) wait_cast(); // só se for skinnando
				 }
				 else
				 {
					loga("Clicou errado ao tentar loot.");
					break;
				 }
				}

				if (loopcount >= max_scans)
				{
				 loga($"Loot finalizado: atingiu limite de {max_scans} scans.");
				}
			 }
			 //-----------------------------------FIM SCANLOOT-----------------------------------

			}
			checkme();
		 }
		 //-----FIM LOOT-----------------------------------

		 // -----------------------------------
		 // HEARTHSTONE SE BAGS FULL / ARMA QUEBRADA 
		 // -----------------------------------
		 else if (cb_hearth_bagfull.Checked) // SEM ESPAÇO NA BAG - HEARTHSTONE ?
		 {
			HS(); // usa a pedra de lar se não tiver espaço na bag
		 }
		 if (me.armabroken) // ARMA QUEBRADA – MAIN OU OFFHAND
		 {
			loga("Arma quebrada detectada. Usando Hearthstone.");
			HS(); // retorna pra reparar
		 }
		 if (cb_hearth_ding.Checked && me.level >= atoi(tb_hearthlevel)) // HEARTHSTONE SE LEVEL UP
			HS(); // usa a pedra de lar se atingiu o level definido
		 

			//if (cb_anda.Checked) aperta(WKEY, 0); // retoma andar se permitido


		 } // FIM DA ROTINA PÓS COMBATE (LOOT / SKIN)

		//----------------------------------
		// RECUPERAÇAO E PREPARO PRÉ COMBATE - PALADINO
		//----------------------------------
		if (me.classe == PALADIN)

		{
		 //---------------------RECUPERA MANA-------------------------
		 if (me.mana < 60) loga($"Esperando recuperação da mana: {me.mana}");

		 while (me.mana < atoi(tb_pull_mana) && !me.combat) // espera recuperar mana
		 {
			if (!pala.bow) aperta(BOW); // buffa se não tiver buff ativo
			wait(1000);
			checkme();
		 }

		}

		//----------------------------------
		// RECUPERAÇÃO E PREPARO PRÉ COMBATE - HUNTER - 
		//----------------------------------
		// Adicionar esta seção na parte de preparo pré-combate (após Warlock):

		else if (me.classe == HUNTER)
		{
		 // espera recuperar HP se necessário
		 if (me.hp < 80) // mesmo limite do Warrior
		 {
			para(); // para de andar se estiver andando
			loga($"Esperando recuperação de HP: {me.hp}");
			aperta(F12); // COMIDA 
			while (me.hp < 80 && !me.combat)
			{
			 espera(1);
			}
		 }

		 
		  if (!hunt.has_pet && me.level >= 10 && !me.combat && hunt.revive_pet_up) // sem pet
		 {
			

			loga("Invocando pet.");

			para(); // para de andar para invocar
		       castslow(SUMMONPET);
		      checkme();
		  }
		}



		//----------------------------------
		// RECUPERAÇAO E PREPARO PRÉ COMBATE - WARRIOR
		//----------------------------------
		else if (me.classe == WARRIOR)
		{
		 // espera recuperar energia
		 if (me.hp < atoi(tb_rest_warr))
		 {
			para(); // para de andar se estiver andando
			loga($"Esperando recuperação de HP: {me.hp}");
			aperta(F12); // COMIDA 
			while (me.hp < atoi(tb_rest_warr) && !me.combat)
			{
			 espera(1);

			}
		 }
		}


		// ================================
		// PREPARO PRÉ-PULL (PET + AURAS)
		// ================================
		else if (me.classe == WARLOCK)
		{
		 // Invoca pet se não tiver
		 if (!wlock.has_pet)
		 {
			loga("Invocando Imp.");
			para(); // para de andar para invocar
			castslow(SUMMONPET);
			checkme();
			if (!wlock.has_pet)
			{
			 loga("Falha ao invocar pet. Continuando sem pet.");
			}
		 }

		 // Ativa Demon Skin se não tiver
		 if (!wlock.has_demon_skin)
		 {
			loga("Ativando Demon Skin.");
			casta(DEMONSKIN);
			checkme();
		 }

		 // CRIAR HEALTHSTONE SE NÃO TIVER
		 if (!wlock.has_healthstone && wlock.create_healthstone_up && wlock.shards >= 1)
		 {
			loga("Criando Healthstone.");
			para(); // para de andar
			castslow(CREATE_HEALTHSTONE);
			checkme();

			if (!wlock.has_healthstone)
			{
			 loga("Falha ao criar Healthstone.");
			}
		 }

		 // LIFE TAP SE MANA BAIXA 
		 if (!cb_lifetap_auto.Checked)
		 {
			while (me.mana < atoi(tb_lifetap_mana) && me.hp > atoi(tb_lifetap_hp) && !me.combat)
			{
			 casta(LIFETAP); // ativa Life Tap
			 checkme(); // atualiza status
			}
		 }
		 else // auto
		 {
			while (me.mana < 85 && me.hp>me.mana && me.hp >= atoi(tb_pull_hp_lock) && !me.combat)
			{
			 casta(LIFETAP); // ativa Life Tap
			 checkme(); // atualiza status
			}
		 }
		 
		 // VIDA BAIXA 
		 while (!me.combat && me.hp < atoi(tb_pull_hp_lock) )
		 {
			espera(1);
			checkme();
		 }


		}

		//----------------------------------
		// RECUPERAÇAO E PREPARO PRÉ COMBATE - ROGUE
		//----------------------------------
		else if (me.classe == ROGUE)
		{
		 // espera recuperar energia
		 if (me.hp < atoi(tb_rogue_eat_at))
		 {
			para(); // para de andar se estiver andando

			if (!rog.stealth && rog.stealth_up)
			 aperta(STEALTH); // ativa stealth se não estiver stealth e stealth_up ativo

			loga($"Esperando recuperação de HP: {me.hp}");
			aperta(F12); // COMIDA 
			while (!me.combat && me.hp < atoi(tb_rogue_eat_at))
			{
			 espera(1);
			}
			if (rog.stealth)
			 aperta(STEALTH); // desativa stealth se stealth ativo
		 }
		}
		
		//--------------------------------------
		// CONTINUA ANDANDO ATÉ CHEGAR NO LOCAL. RESTART DO LOOP

	 } while (on && dist(me.pos, destino) > (catando_planta ? 70 : 120)); // enquanto ativo e longe (20 se catando)

	 loga("Waypoint atingido. Partindo para proximo.");
	 if (catando_planta) catando_planta= false; // chegou na planta 

	 //aperta(WKEY, 2); // solta W ao chegar no destino
	}
	bool catando_planta = false;
	//--------------------------------------------
	/// MÉTODO HEARTHHOME - USADO PARA VOLTAR PARA A CIDADE COM HEARTHSTONE
	//--------------------------------------------
	void HS()
	{
	 para(); // para de andar se estiver andando
	 aperta(HEARTHSTONE); // Casta hearth 
	 int segundos = 0; // timer para hearthstone 
	 loc origin = me.pos; // de onde eu saí 
	 loga("Usando Hearthstone. ");
	 while (!me.combat && segundos < 20) // espera hearth 
	 {
		wait(1000);
		checkme();
		segundos++;
	 }
	 if (dist(me.pos, origin) > 200) // foi para bem longe 
	 {
		loga("Teleport confirmado, encerrando o bot");
		Environment.Exit(0);
	 }
	}




void andaplanta(loc alvo)
	{
	 solta(ANDA); // para de andar
	 aperta(F2); // anda devagar ate a planta 
	 int ticker = 0; // contador de ticks
	 press(ANDA); // anda devagar até a planta
	 
		while (dist(me.pos, alvo) > 10)
	 {
		checkme();
		wait(500);
		giralvo(alvo); // gira para a planta
		 if (ticker % 5 == 0) aperta(PULA); // pula
		 if (me.combat || ticker++ > 30) break; // se entrar em combate, sai do loop
	 }
	 solta(ANDA); // para de andar
	 aperta(F2); // Volta andar rapido 

	}


	// --------------------------------------------
	// MÉTODO CATAPLANTA - CATA PLANTA
	// --------------------------------------------
	void cataplanta(loc planta)
	{
	 catando_planta = true; // ativa flag de catando planta
	 
	 moveto(planta); // anda até perto da planta
	 andaplanta(planta); // anda devagar até a planta





	 loga("Cheguei na planta. Catando planta: " + planta.x + "," + planta.y);
	 solta(ANDA); // para de andar
	 wait(1000); // espera 1 segundo
	 checkme();

	 loc p = scanloot();        // tenta encontrar algo clicável
	 if (p.x < 0) return;        // nada encontrado → encerra

	 clica(p);                  // clica com botão direito
	 wait(100);
	 checkme();

	 if (me.spd == 0 && !me.combat) // se continuou parado e sem combate
	 {
		wait(1200);                 // espera pelo loot ou skin
		if (cb_herbalism.Checked) wait_cast(); // espera skin só se tiver ticado 

		foreach (var item in hash_planta) // remove a planta (ou clones) da hashset pra poder pegar de novo se respawnar
		{
		 if (dist(item, planta) < 20)
		 {
			hash_planta.Remove(p);
			break; // evita "Collection was modified" se tiver mais de um
		 }
		}

	 }
	 else
	 {
		loga("Clicou errado ao tentar loot.");
		return; // se clicou no mundo e saiu andando 
	 }


	}

	// --------------------------------------------
	// MÉTODO BLESS: APLICA O BUFF PALADINO ADEQUADO
	// --------------------------------------------
	public void bless(element e)
	{
	 checkme(); // atualiza status do jogador e buffs

	 int bless = 0; // 0 = nenhum, 1 = BOM, 2 = BOW, 3 = BOK, 4 = BOSA

	 // --------------------------------------------
	 // BLOQUEIA TROCA SE FOR BOW E MANA < 35%
	 // --------------------------------------------
	 if (pala.bow && !mana(35)) return; // única situação que bloqueia

	 // --------------------------------------------
	 // PARAMETROS EXTERNOS
	 // --------------------------------------------
	 int limiar_bosa = int.Parse(tb_BOSA_limiar.Text); // hp para ativar BOSA
	 int limiar_bow = int.Parse(tb_bow_trig.Text);      // mana para ativar BOW fora de combate

	 // --------------------------------------------
	 // MODO COMBATE
	 // --------------------------------------------
	 if (me.combat)
	 {
		// REGIÃO 1: MANA < 20% → ENTRA EM MODO SEM MANAd
		if (!pala.nomana && !mana(20) && cb_BOW.Checked)
		{
		 bless = 2;
		 pala.nomana = true; // trava BOW até o fim do combate
		}
		else if (pala.nomana && cb_BOW.Checked)
		{
		 bless = 2; // continua usando BOW até fim do combate
		}
		// REGIÃO 2: HP BAIXO → ENTRA EM MODO DEFENSIVO
		else if (!pala.defbless && me.hp <= limiar_bosa && cb_BOSA.Checked)
		{
		 bless = 4;               // aplica BOSA
		 pala.defbless = true;   // ativa modo defensivo
		}
		// REGIÃO 3: MODO NORMAL OU DEFENSIVO JÁ ATIVO
		else if (pala.defbless)
		{
		 bless = 4; // mantém BOSA enquanto estiver em modo defensivo
		}
		else
		{
		 if (cb_BOK.Checked && cb_BOM.Checked)
			bless = 3;
		 else if (cb_BOM.Checked)
			bless = 1;
		 else if (cb_BOK.Checked)
			bless = 3;
		 else if (cb_BOW.Checked)
			bless = 2;

		 // evita trocar Wisdom por buff ofensivo se mana < 90%
		 if ((bless == 1 || bless == 3) && pala.bow && !mana(atoi(tb_disable_BOW)))
			bless = 2; // mantém Blessing of Wisdom
		}
	 }
	 // --------------------------------------------
	 // FORA DE COMBATE
	 // --------------------------------------------
	 else
	 {
		bool mana_baixa = !mana(limiar_bow); // true se mana < limiar externo

		if (mana_baixa && cb_BOW.Checked)
		 bless = 2;
		else if (cb_BOK.Checked && cb_BOM.Checked)
		 bless = 3;
		else if (cb_BOM.Checked)
		 bless = 1;
		else if (cb_BOK.Checked)
		 bless = 3;
		else if (cb_BOW.Checked)
		 bless = 2;

		// evita trocar Wisdom por buff ofensivo se mana < 80%
		if ((bless == 1 || bless == 3) && pala.bow && !mana(80))
		 bless = 2; // mantém Blessing of Wisdom

		// RESET DE ESTADOS DEFENSIVOS
		pala.defbless = false;
		pala.nomana = false;
	 }

	 // --------------------------------------------
	 // EVITA RECASTAR BLESSING JÁ ATIVA
	 // --------------------------------------------
	 if ((bless == 1 && pala.bom) ||
		 (bless == 2 && pala.bow) ||
		 (bless == 3 && pala.bok) ||
		 (bless == 4 && pala.bosanc))
		return;

	 // --------------------------------------------
	 // APLICA O BUFF DEFINIDO
	 // --------------------------------------------
	 if (bless == 1) aperta(BOM);
	 else if (bless == 2) aperta(BOW);
	 else if (bless == 3) aperta(BOK);
	 else if (bless == 4) aperta(BOSA);

	 checkme(); // atualiza status final
	}



	// ----------------------------------------
	// MÉTODO scan_elites
	// Verifica se o target é elite, raro ou patrulha e aguarda se necessário
	// ----------------------------------------
	void scan_elites()
	{
	 if (!cb_scan_elite.Checked) return; 



	 string tipo = "";  // tipo textual do alvo

	 if (tar.type == 50 && cb_humanoid_patrol.Checked)
		tipo = "humanoide";
	 else if (tar.type == 240 && cb_giant_patrol.Checked)
		tipo = "gigante";
	 else if (tar.israre && cb_rare_patrol.Checked)
		tipo = "raro";
	 else if (tar.iselite && cb_elite_patrol.Checked)
		tipo = "elite";
	 else if (tar.level > me.level + 1 && cb_scan_highlevel.Checked && tar.mood != 1)
		tipo = "highlevel"; // alvo de nível alto
	 else if (tar.type == ELEMENTAL && elemental_patrol.Checked)
		tipo = "elemental"; // alvo de nível alto

	 if (me.hastarget && tipo != "")
	 {
		para(); // para de andar se estiver andando
		int seg = atoi(tb_wait_patrol); // segundos a esperar
		loga($"Detectado patrulha do tipo {tipo}. Esperando {seg} segundos.");
		if (me.classe == ROGUE && !rog.stealth)
		{
		 aperta(STEALTH); // entra em stealth se for rogue e não estiver stealth
		 clog("Stealth anti patrol ativado.");
		}
		espera(seg); // aguarda o tempo configurado
	 }
	 //else
	//	loga("Nenhum elite ou patrulha detectado.");
	}


		 bool deucharge = false; // flag para indicar se Charge foi usada
	// --------------------------------------------
	// MÉTODO puxa - Versão Paladino com verificação organizada
	// Inicia o combate apenas se o target for válido
	// --------------------------------------------
	void puxa()
	{
	 Func<bool> has_seal = () => pala.sor || pala.soc || pala.sow || pala.sol || pala.sotc; // verifica se tem algum Seal
	 Func<int, bool> mana = (p) => me.mana > p;

	 checkme();                                                                 // atualiza status depois do tab

	 // NAO DEIXA AFOGAR 
	 //------------------------------
	 nao_afoga(); // nada para cima se estiver afogando



	 // --------------------------------------------
	 // VERIFICAÇÃO DO TARGET - ESCOLHA ENTRE DOIS
	 // --------------------------------------------

	 // --------------------------------------------
	 // VERIFICAÇÃO DO TARGET - ESCOLHA ENTRE DOIS
	 // --------------------------------------------
	 bool bomtarget()
	 {
		// Delay curto para garantir leitura completa
		//wait(50);  // 50ms de delay
							 // Rejeita target inválido ou sem tipo
		if (!me.hastarget || tar.type == 0) return false;

		// Log de depuração
		//loga($"🕵️ Verificando target: Type={tar.type}, CB_NoHumanoid={cb_nohumanoid.Checked}");

		if (cb_pacifist.Checked) return false;
		if (tar.hp == 0 || tar.morreu) return false;
		if (isgray(me.level, tar.level) && !cb_killgray.Checked) return false;
		if (tar.mood == 1) return false; // amarelo = não hostil
		if (tar.iselite && cb_noelite.Checked) return false; // não é elite se não estiver marcado
		if (tar.hp < 100) return false; // já apanhou
		if (tar.level > me.level + Convert.ToInt32(tb_pullcap.Text)) return false;
		if (cb_nomurloc.Checked && tar.type == MURLOC) return false;

		// Bloqueia apenas humanóides NPC
		if (cb_nohumanoid.Checked && tar.type == HUMANOID)
		{
		 loga($"🚫 Humanóide rejeitado: Type={tar.type}");
		 return false;
		}

		if (cb_nodragonkin.Checked && tar.type == DRAGONKIN) return false;
		if (cb_nomech.Checked && tar.type == MECHANICAL) return false;
		

		return true;
	 }
	 checkme();
	 // pega primeiro target
	 
	 
		aperta(TAB, 120);
		checkme(); // atualiza status do segundo target
		if (me.hastarget) loga("Pegando target. Code 1.");

	 int mood1 = tar.mood;
	 bool t1_ok = bomtarget();
	 int t1_id = tar.id;
	 scan_elites(); // verifica se tem elite no target e ajusta o pull se necessário
	 checkme(); // atualiza status após o TAB inicial
							// pega segundo target
	 loga($"Target 1 mood: {mood1}");
	 aperta(TAB, 120);
	 checkme();
	 if (me.hastarget) loga("Pegando target. Code 2.");

	 int mood2 = tar.mood;
	 loga($"Target 2 mood: {mood2}");
	 bool t2_ok = bomtarget();
	 int t2_id = tar.id;
	 if (t2_id != t1_id) scan_elites(); // verifica se tem target novo
	 if (t1_id == 0) // se nenhum target foi encontrado inicialmente
	 loga("Procurando targets.");
	 else if (!t1_ok && !t2_ok) // nenhum dos dois é válido
	 {
		loga("Nenhum dos dois targets é válido. Limpando o target.");
		aperta(CLEARTGT);
	 }
	 else if (t1_id == t2_id) // TAB não achou outro mob, manteve o mesmo
		loga("Um target localizado.");
	 else if (t1_ok && !t2_ok) // só o primeiro é bom
	 {
		loga("Apenas o primeiro target é válido. Retornando para ele.");
		aperta(TARGETLAST);
	 }
	 else if (!t1_ok && t2_ok) // só o segundo é bom
		loga("Apenas o segundo target é válido. Mantido.");
	 else if (mood1 == mood2) // mesmo comportamento (hostis ou pacíficos)
	 {

		if ((cb_prefer_distant.Checked && me.classe == HUNTER && mood1 == 0))
		 loga("Escolhendo o mais distante para ranged class.");
		else
		{
		 loga("Dois targets com mesmo comportamento. Priorizando o mais próximo.");
		 aperta(TARGETLAST);
		}
		
	 }
	 else if (mood1 < mood2) // primeiro é mais hostil
	 {
		loga("Primeiro target é mais hostil que o segundo. Retornando para ele.");
		aperta(TARGETLAST);
	 }
	 else if (mood2 < mood1) // segundo é mais hostil
		loga("Segundo target é mais hostil que o primeiro. Mantido.");
	 else // empate total ou condição rara
	 {
		loga("Empate total inesperado. Escolhendo o primeiro.");
		aperta(TARGETLAST);
	 }
	 checkme(); // atualiza status após a escolha do target
	 if (cb_nomurloc.Checked && tar.type == MURLOC)
	 {
		aperta(CLEARTGT);
		return;
	 }

	 
		// --------------------------------------------
		// CORRE ATE O MOB E ATACA (PULL)
		// --------------------------------------------
		int ticker = 0; // contador de ciclos

		if (me.hastarget && tar.hp == 100 && !me.combat) // alvo válido e fora de combate
		{
		loga($"Target type: {tar.type}.");
		loga("Target locked. Iniciando pull.");
		 do // while (me.hastarget && !me.combat && tar.hp == 100) // alvo válido e ainda fora de combate
		 {
			checkme();

			// NAO DEIXA AFOGAR 
			//------------------------------
			nao_afoga(); // nada para cima se estiver afogando

			//------------------------------- PULL PALADINO ----------------
			if (me.classe == PALADIN)
			{
			 aperta(best_seal()); // aplica o melhor seal fora de combate
														// verifica se pode puxar com EXORCISM
			 bool pode_exorcism = cb_use_exorcism.Checked
				 && me.level >= 20
				 && (tar.type == 150 || tar.type == 200)  // undead ou demon
				 && pala.exorcism_up
				 && pala.exorcism_range
				 && mana(8);

			 // se pode, usa EXORCISM no lugar do Judgement
			 if (pode_exorcism)
			 {
				aperta(PULA);        // interrompe qualquer cast se necessário
				aperta(EXORCISM);        // tecla de Exorcism
				aperta(AUTOATTACK);  // inicia ataque automático
			 }
			 // senão, tenta puxar com JUDGEMENT
			 else if (
				 me.level >= 4 &&
				 has_seal() &&
				 pala.jud_range &&
				 pala.judge_cd == 0 &&
				 mana(4))
			 {
				aperta(PULA);
			 viramob(); // gira para o mob
			 aperta(JUDGEMENT);
				aperta(AUTOATTACK);
			 viramob();
			 }
			 else
			 {
				aperta(AUTOATTACK); // ataca direto se nenhuma opção acima for válida
				aperta(INTERACT); // anda até o mob11
			 loga("Pala interact 112");
			 }

			 if (ticker++ % 8 == 0)
				aperta(PULA); // pulo de simulação humana a cada 2s

			 aperta(INTERACT); // anda até o mob11
			//loga("Pala interact 113");
			wait(200);   // aguarda meio segundo
			 checkme();   // atualiza status
			}
			// ---------------------------------------------
			// WARRIOR
			// ---------------------------------------------
			else if (me.classe == WARRIOR) // se for warrior
			{
			 //aperta(AUTOATTACK);    // ativa autoattack
			 aperta(INTERACT);      // começa a andar até o mob
			 //loga("Pala interact 114");
			checkme();             // atualiza status

			 // ---------------------------------------------
			 // PULL COM ARREMESSO (Throw)
			 // ---------------------------------------------
			 if (war.throw_up && cb_war_rangepull.Checked) // se pode usar arremesso (range, cooldown etc.)
			 {

				// ---------------------------------------------
				// PULL COM FACA
				// ---------------------------------------------
				loga("Parando para usar besta.");
				para(); // para de andar

				if (!war.throw_up) // perdeu o range, FALLBACK PARA CHARGE
				{
				 loga("Muito perto da faca. Avaliando Stealth.");
				 if (war.charge_up)
				 {
					loga("Dando Charge após perder range da besta");
					aperta(CHARGE);
					espera(1);
					if (me.melee) return; // se entrou em combate, sai do loop
				 }
				 aperta(INTERACT); // segue andando stealth ou normal
				}
				else
				{
				 casta(THROW); // Usa best 
				 for (int i = 0; i < 15; i++) // 10 ciclos de 500ms = 5 segundos
				 {
					wait(300);      // espera 0.5s
					checkme();      // atualiza status
					if (me.melee || me.mobs > 0)  // se entrou em combate, sai do loop
					 break;
				 }
				 if (me.combat)
				 {
					return; // sai do pull se nao conseguiu engajar
				 }
				 else aperta(CLEARTGT);

				}






			 }
			 // ---------------------------------------------
			 // PULL COM CHARGE
			 // ---------------------------------------------
			 else if (war.charge_up)
			 {
				clog("Pull com charge.");

				aperta(ANDA); // retoma andar após Charge	
				casta(CHARGE);    // usa Charge
				deucharge = true; // ativa flag de Charge usada
				aperta(AUTOATTACK);
				for (int i = 0; i < 10; i++) // espera 5s para entrar em combate
				{
				 wait(500); // espera 0.3s para evitar problemas de lag
				 checkme();
				 if (me.combat || me.melee) return; // se entrou em combate, sai do loop
				}


			 }

			 // ---------------------------------------------
			 // PULL COM HEROIC STRIKE OU BODY PULL
			 // ---------------------------------------------
			 if (war.hs_up) aperta(HEROICS); // se já tem rage, tenta abrir com HS

			 checkme();
			 if (me.combat) return;

			 if (ticker++ % 16 == 0) aperta(PULA); // pulo humano a cada ~2s
			 aperta(INTERACT); // continua andando até o mob
			}

		 // --------------------------------------
		 // PULL HUNTER REFEITO - WOW CLASSIC HC
		 // --------------------------------------
		 else if (me.classe == HUNTER) // inicio pull hunter
		 {
			loga("Iniciando pull como Hunter.");
			aperta(AUTOATTACK); // ativa auto attack
			aperta(INTERACT); // começa a andar para o mob
			int t0 = Environment.TickCount; // marca o tempo inicial
			int lastInteract = t0; // controle de interações
			int lastJump = t0; // controle de pulos
			bool jafalou = false; // flag de log
			while (!me.combat && Environment.TickCount - t0 < 6000 && me.hastarget && tar.hp >= 100)
			{
			 checkme(); // atualiza estado
			 if (check_stuck() >= 2) break; // verifica stuck
																			// verifica se auto shot está pronto para ranged pull
			 if (hunt.auto_shot_up)
			 {
				clog("Em range do Auto Shot. Executando ranged pull.");
				para(); // para de andar
				checkme();
				if (hunt.auto_shot_up && !hunt.auto_shot_ativo) // se não está ativo, ativa
				{
				 if (me.level >= 8 && cb_huntersmark.Checked && !tar.trivial) casta(HUNTERSMARK);
				 casta(AUTOSHOT);
				 
				 loga("Auto Shot ativado.");
				 wait(1000); // espera 2s para garantir que o Auto Shot está ativo
				}
				clog("Esperando aproximação do mob após ranged pull.");
				// espera ativa de até 6s
				while (!me.melee && me.mobs == 0 && Environment.TickCount - t0 < 10000)
				{
				 checkme(); // atualiza estado
										// se auto shot parou, já prepara o raptor strike
				 if (!hunt.auto_shot_range_ok && hunt.raptor_strike_up)
				 {
					aperta(RAPTORS);
					clog($"Sem range após {(Environment.TickCount - t0) / 1000}s! Preparando Raptor Strike.");
					break;
				 }

				 // se tiver arcane shot pronto, pode castar aqui
				 if (hunt.serpent_sting_up)
				 {
					aperta(SERPENTSTING); // (comentado: ainda não aprendeu)
					clog("Casting Serpent Sting.");
				 }
				 else if (hunt.arcane_shot_up)
				 {
					aperta(ARCANESHOT); // (comentado: ainda não aprendeu)
					clog("Casting Arcane Shot.");
				 }
			 

				 // se tiver serpent sting pronto, pode castar aqui
				 
				}
				if (me.melee || me.mobs > 0)
				{
				 if (hunt.raptor_strike_up)
				 {
					aperta(RAPTORS); // se não está no dead zone, usa Raptor Strike
					aperta(INTERACT); // engaja mob
					loga("Usando Raptor Strike em melee range.");
				 }
				 break; // mob chegou
				}
				if (me.combat) // se combate iniciou após ranged
				{
				 loga("Combate iniciado após ranged pull.");
				 return;
				}
			 }
			 // fallback para melee caso não tenha ranged
			 if (me.melee)
			 {
				if (hunt.raptor_strike_up)
				{
				 aperta(RAPTORS);
				 loga("Usando Raptor Strike em melee range.");
				}
				else
				{
				 if (!me.autoattack)
					aperta(AUTOATTACK);
				}
			 }
			 // movimento contínuo: interact e pulo
			 int now = Environment.TickCount;
			 if (now - lastInteract >= 500)
			 {
				aperta(INTERACT);
				lastInteract = now;
			 }
			 if (now - lastJump >= 3000)
			 {
				aperta(PULA, 20);
				lastJump = now;
			 }
			}
			// avaliação final do pull
			if (!hunt.auto_shot_range_ok || me.melee || me.mobs > 0)
			{
			 para();
			 //aperta(PULA, 10);
			 //aperta(INTERACT);
			 checkme();
			 if (hunt.raptor_strike_up)
			 {
				aperta(RAPTORS); // usa Raptor Strike se não estiver no dead zone
				clog("Last chance raptor strike.");
			 }
			 clog("Pull concluído com sucesso.");
			 return;
			}
			else
			{
			 clog("Pull falhou. Não entrou em combate.");
			 aperta(CLEARTGT);
			}
		 } // fim pull hunter


		 //------------------------------------------------------------
		 // ---------------------INICIO PULL ROGUE ----------------
		 //--------------------------------------------------------
		 else if (me.classe == ROGUE) // ---------------- INÍCIO PULL ROGUE ----------------
		 {
			loga("Iniciando pull como Rogue.");
			aperta(AUTOATTACK);
			aperta(INTERACT);                      // inicia aproximação
			int t0 = Environment.TickCount;        // começa o timer global do pull
			int lastInteract = t0;
			int lastJump = t0;
			bool jafalou= false; // flag para saber se já avisou
			while (!me.combat && Environment.TickCount - t0 < 15000 && me.hastarget && tar.hp >= 100)
			{
			 checkme();
			 if (check_stuck() >= 2) break;
			 // Se entrou em range do THROW
			 if (rog.throw_up)
			 {
				if (!jafalou && !cb_range_pull.Checked && cb_stealth_pull.Checked && tar.trivial)
				{
				 loga("Target trivial, não usando stealth.");
				 jafalou = true; // avisa apenas uma vez
				}
				if (cb_range_pull.Checked)
				{
				 loga("Em range do ranged. Executando ranged pull.");
				 para();
				 if (rog.throw_up)
				 {
					casta(THROW);

					// Espera ativa após o Throw, até 15s totais
					loga("Esperando reação do mob após ranged pull.");
					while (Environment.TickCount - t0 < 15000)
					{
					 //if (check_stuck() >= 2) break;
					 wait(300);
					 checkme();
					 if (me.melee || me.mobs > 0)
						break;
					}
				 }
				 else loga("Perdeu range do ranged pull. Fallback para melee pull.");
				 if (me.combat)
				 {
					loga("Combate iniciado após ranged pull.");
					return;
				 }
				}
				else if (cb_stealth_pull.Checked && rog.stealth_up && !tar.trivial && !rog.stealth)
				{
				 loga("Ranged desativado. Entrando em Stealth.");
				 para();
				 aperta(STEALTH);
				 wait(400); // dá tempo pro buff aplicar
				 checkme();
				}
			 }
			
			 // Se estiver em melee range
			 if (me.melee)
			 {
				if (tar.type == HUMANOID && cb_pickpocket.Checked && rog.stealth)
				 aperta(PICKPOCKET);
				else
				{ // sinister strike com log 
				 int preen = me.mana;
				 int prehp = tar.hp;
				 aperta(SS);  // seu código existente
				 checkme();
				 int dano_ss = prehp - tar.hp;
				 int managasta = preen - me.mana; // energia gasta pelo SS
																					// Registra energia e dano do SS
				 if (managasta > 15)
					ComboOptimizer.RegistrarCast(
							atoi(tb_energy_ss),
							(int)Math.Round(dano_ss * ((double)atoi(tb_damage_ss) - (double)atoi(tb_damage_hit)) / (double)atoi(tb_damage_ss))
					);
	
				}
			 }

			 // Movimento contínuo
			 int now = Environment.TickCount;
			 if (now - lastInteract >= 500)
			 {
				aperta(INTERACT);
				lastInteract = now;
			 }
			 if (now - lastJump >= 3000)
			 {
				aperta(PULA, 20);
				lastJump = now;
			 }
			}

			// Avaliação final: combate iniciou ou não
			if (me.combat)
			{
			 para();
			 aperta(PULA, 10);
			 aperta(INTERACT);
			 clog("Pull concluído com sucesso.");
			 return;
			}
			else
			{
			 clog("Pull falhou. Não entrou em combate.");
			 if (rog.stealth) aperta(STEALTH); // toggle stealth para sair
			 para();
			 // não limpa o target aqui — pode haver fallback fora
			}
		 } // ---------------- FIM PULL ROGUE ----------------

		 //------------------------------------------------------------
		 // ---------------------INICIO PULL WARLOCK ----------------
		 //--------------------------------------------------------
		 else if (me.classe == WARLOCK) // ---------------- INÍCIO PULL WARLOCK ----------------
		 {
			// ================================
			// CONTROLE DE PULOS (ANTI-TRAVAMENTO)
			// ================================
			 int warlock_ticker = 0; // contador estático para persistir entre chamadas
			warlock_ticker++;

			// Log apenas na primeira vez ou a cada 10 ciclos para evitar spam
			if (warlock_ticker == 1 || warlock_ticker % 10 == 0)
			 clog("Iniciando pull.");

			// PULO HUMANO a cada 3 segundos (~15 ciclos de 200ms)
			if (warlock_ticker % 15 == 0)
			{
			 aperta(PULA); // pulo para superar obstáculos
			 loga("Pulo anti-travamento.");
			}

			// ================================
			// PULL RANGED  
			// ================================
			//VIRA PARA O MOB
			aperta(INTERACT, 400); // começa a andar até o mob

			// Verifica se pode puxar com Shadow Bolt
			if (wlock.shadowbolt_up || wlock.immolate_up || me.wand_up)
			{
			 loga("Parando para castar.");
			 if (cb_sendpet.Checked && wlock.has_pet)
			 {
				aperta(PETATTACK); // manda o pet atacar se checkbox ativo
				clog("Mandando pet atacar.");
			 }
			 para(); // para de andar

			 // Verifica se ainda tem range após parar
			 checkme();
			 int myskill = SHADOWBOLT;
			 bool usar_wand = false;
			 bool fazer_nada = false;

			 // ================================
			 // ESCOLHA DA SKILL PRIORITÁRIA
			 // ================================
			 // Prioridade 1: Immolate (se checkbox ativo e spell disponível)
			 if (cb_use_immolate.Checked && wlock.immolate_up )
			 {
				myskill = IMMOLATE;
			 }
			 // Prioridade 2: Shadow Bolt (se checkbox ativo e spell disponível)
			 else if (cb_use_shadowbolt.Checked && wlock.shadowbolt_up)
			 {
				myskill = SHADOWBOLT;
			 }
			 // Prioridade 3: Wand (se spells indisponíveis ou desabilitadas)
			 else if (me.wand_up)
			 {
				usar_wand = true;
				clog("Usando Wand (last choice)");
			 }
			 // Última opção: Fazer nada (fallback melee embaixo)
			 else
			 {
				fazer_nada = true;
				loga("Nenhuma opção ranged disponível. Fallback para melee.");
			 }

			 // ================================
			 // EXECUÇÃO DA SKILL ESCOLHIDA
			 // ================================
			 if (fazer_nada)
			 {
				// Não faz nada - deixa o fallback melee embaixo cuidar
				loga("Continuando aproximação melee.");
			 }
			 else if (!usar_wand) // Spells normais
			 {
				if (cb_sendpet.Checked) aperta(PETATTACK);// MANDA O PET
				aperta((byte)myskill, 2000); // usa spell
				checkme(); // atualiza status após o cast

				if (me.wrongway)
				{
				 
				 aperta(INTERACT, 200);
				 para(); // para de andar após o cast
				 aperta((byte)myskill, 2000); // tenta novamente se andou errado
				}

				// Espera entrar em combate
				wait(500);
				checkme();

				if (me.combat)
				{
				 loga("Pull com sucesso! Entrando em combate.");
				 warlock_ticker = 0; // reseta contador ao entrar em combate
				 return;
				}
				else
				{
				 loga("Spell não iniciou combate. Continuando aproximação.");
				}
			 }
			 else // Usar Wand
			 {
				if (cb_sendpet.Checked) aperta(PETATTACK);// MANDA O PET
				casta(WAND); // ativa wand

				clog("Cast: Wand.");

				checkme();
				if (me.combat)
				{
				 clog("Pull com Wand bem-sucedido!");
				 warlock_ticker = 0; // reseta contador ao entrar em combate
				 return;
				}
				else
				{
				 loga("Wand não iniciou combate. Continuando aproximação.");
				}
			 }
			}

			// ================================
			// FALLBACK: APROXIMAÇÃO MELEE
			// ================================
			// Se não conseguiu puxar ranged, vai melee
			if (!me.combat)
			{
			 aperta(INTERACT); // continua andando até o mob
			}

			// ================================
			// TIMEOUT DE SEGURANÇA
			// ================================
			if (warlock_ticker > 50) // ~10 segundos sem sucesso
			{
			 loga("Pull timeout - limpando target.");
			 aperta(CLEARTGT);
			 warlock_ticker = 0; // reseta contador
			}
		 }//------------------- FIM PULL WARLOCK ----------------
			// --------------------------------
			// PRIEST PULL ROUTINE
			// --------------------------------
		 else if (me.classe == PRIEST)
		 {
			DateTime inicio = DateTime.Now;
			loga($"Priest Pull: Iniciando pull no target {tar.level}");

			void shieldme()
			{
			 // Casta PWS se disponível
			 if (cb_shielded_pull.Checked && priest.pws_up && (!priest.has_pws || !cb_combat_pws.Checked) && !priest.has_weakened_soul)
			 {
				loga("Priest Pull: Aplicando Power Word: Shield");
				casta(PWS);
				checkme(); // atualiza status após o cast
				if (!me.autoattack) aperta(AUTOATTACK); // ativa autoattack se não estiver ativo
			 }
			}

			while (!me.combat && (DateTime.Now - inicio).TotalSeconds < 10)
			{
			 checkme();

			 // Se entrou em combate, sair
			 if (me.combat)
			 {
				loga("Priest Pull: Entrou em combate");
				return;
			 }

			 // Se perdeu o target, limpar e sair
			 if (!me.hastarget)
			 {
				loga("Priest Pull: Target perdido");
				return;
			 }

			 // Aproximar do target
			 aperta(INTERACT,200);

			 // Se estiver em range, castar spell de pull
			 if (me.wand_up || priest.smite_up || priest.swp_up)
			 {
				para(); // para de andar
        
				shieldme(); // aplica PWS se checkbox ativo
				// Preferir Shadow Word: Pain se disponível
				if (priest.swp_up && !priest.has_swp)
				{
				 clog("Priest Pull: Usando Shadow Word: Pain");
				 casta(SWP);
				 espera(2); // espera 2 segundos para ver se entra em combate
				 if (me.combat)
				 {
					loga("Pull com SWP com sucesso. Saindo do pull.");
					
					return; // se entrou em combate, sai do loop
				 }
				  }
				// Senão usar Smite
				else if (priest.smite_up)
				{
				 clog("Priest Pull: Usando Smite");
				 viramob(); // vira para o mob antes de castar
				 viramob(); // vira para o mob antes de castar
				 castslow(SMITE);
				 espera(2);
				 if (me.combat)
				 {
					loga("Pull com smite com sucesso. Saindo do pull.");
					
					return; // se entrou em combate, sai do loop
				 }
				}
				else if (me.wand_up)
				{
				 casta(WAND); // ativa wand
				 espera(3);
				 if (me.combat)
				 {
					loga("Pull com WAND com sucesso. Saindo do pull.");
					
					return; // se entrou em combate, sai do loop
				 }
				}
				
				// Espera um pouco para ver se entra em combate
				wait(500);
				checkme();
				if (me.combat)
				{
				 
				 return;
				}
			 }
			 checkme();
			 wait(100);
			}
			loga("Sem pull ranged disponível. Fallback.");
			// Se chegou aqui, timeout - limpar target
			if (!me.combat)
			{
			 loga("Priest Pull: Timeout - limpando target");
			 aperta(CLEARTGT); // clear target
			}
		 }//------------------- FIM PULL PRIEST ----------------


		 checkme();
			if (ticker > 10 && !me.combat) // passou 5s e não entrou em combate
			{
			 aperta(CLEARTGT); // limpa target e aborta pull
			 break;
			}
		 } while (me.hastarget && !me.combat && tar.hp == 100); // alvo válido e ainda fora de combate

		 if (me.combat) para();


		}
	 }
	//------------
	// METODO PARA
	//------------
	void para()
	{
	 if (me.spd > 0) // se estiver andando
	 {
		aperta(SKEY,20); // pequeno toque para trás
		solta(WKEY); // para de andar para frente
		checkme(); // atualiza status
	 }
	 while (me.spd > 0) // enquanto estiver andando
	 {
		solta(WKEY); // para de andar
		solta(SKEY); // para de andar para trás
		solta(AKEY); // para de andar para a esquerda
		solta(DKEY); // para de andar para a direita
		checkme(); // atualiza status
	 }
	}
	// --------------------------------
	// ETAPA 3: TRACKING DURANTE COMBATE
	// Incluir no LOOP DO COMBATE (dentro do while), ANTES das rotinas de classe
	// --------------------------------
	private void rastreia_mob_combate()
	{
	 // só adiciona se target válido, com aggro, e levou porrada significativa
	 if (me.hastarget && (tar.player_aggro || tar.pet_aggro ) && tar.hp < 50)
	 {
		// verifica se é mob skinnable
		if (tar.type == HUMANOID || tar.type == BEAST || tar.type == DEMON || tar.type == DRAGONKIN)
		{
		 if (killed_skin.Add(tar.id)) // Add retorna true se foi novo elemento
		 {
			loga($"Mob skinnable rastreado: ID {tar.id}, tipo {tar.type}");
		 }
		}
		else
		{
		 if (killed_noskin.Add(tar.id)) // Add retorna true se foi novo elemento  
		 {
			loga($"Mob normal rastreado: ID {tar.id}, tipo {tar.type}");
		 }
		}
	 }
	}

	// --------------------------------
	// ETAPA 4: CÁLCULO INTELIGENTE DE SCANS
	// Substituir a linha "if (loopcount++ >= 6)" no método de loot
	// --------------------------------
	private int calcula_max_scans()
	{
	 int max_scans;

	 if (cb_skinning.Checked)
	 {
		// Skinning ATIVO: mobs skinnable precisam de 2 scans (loot + skin)
		max_scans = killed_noskin.Count + (killed_skin.Count * 2);
	 }
	 else
	 {
		// Skinning DESATIVO: todos os mobs precisam só de 1 scan (loot apenas)
		max_scans = killed_noskin.Count + killed_skin.Count;
	 }

	 // proteção: mínimo 1 scan, máximo 8 (backup de segurança)
	 max_scans = Math.Max(1, Math.Min(max_scans, 8));

	 loga($"Mobs mortos: {killed_noskin.Count} normais + {killed_skin.Count} skinnable = {max_scans} scans máximos");

	  return max_scans;
	}



	// --------------------------------
	// MÉTODO VIRAMOB - USADO POR MÚLTIPLAS CLASSES
	// --------------------------------
	void viramob()
	{
	 if (tar.player_aggro || tar.pet_aggro ) // se tem aggro válido
		aperta(INTERACT, 300);
	 if (me.iscaster) para(); // para de andar antes de castar
	}
	// --------------------------------
	// MÉTODO COMBATLOOP - ROTINAS DE COMBATE
	// --------------------------------
	// Variáveis de controle
	long last_purify = 0; // armazena o último uso do purify em ms
	int last_kill_time = Environment.TickCount; // tempo do último combate vencido
																							// --------------------------------

	public void combatloop()
	{
	 loga("Iniciando loop de combate Combatloop().");
	 checkme(); // atualiza status inicial
	 para(); // para de andar se estiver andando
	 int combat_ticker = 0; // contador de ciclos de combate
	 int generic_ticker = 0; // contador de ciclos genérico
	 bool ja_deu_backpedal = false; // se estiver apanhando muito anda um pouco para tras pra nao dar as costas 
	 bool jalogou = false; // se já logou o decay
	 if (!emCombate) // decay
	 {
		decay.Start(me.hp);
		emCombate = true;
	 }
	 int ticker = 0; // contador de ciclos
	 //----------------------------------
	 // ROTINAS PRÉ LOOP DE COMBATE
	 //-----------------------------------
	 // LIMPA MOB COUNT (USADO PARA LOOT E SKIN) 
	  killed_skin.Clear();        // zera lista de mobs skinnable
    killed_noskin.Clear();      // zera lista de mobs normais
    loga("Tracking de mobs reiniciado para novo combate.");
	 
	 do // while me.combat
	 {
		
		if (!on) return; // se o bot estiver desligado, sai do loop
		combat_ticker++; // incrementa contador de combate 
		ticker++;
		Func<bool> has_seal = () => pala.sor || pala.soc || pala.sow || pala.sol || pala.sotc; // verifica se tem algum seal ativo
		Func<int, bool> mana = (p) => me.mana > p;

		if (me.combat) // calculo do decay
		{
		 decay.Update(me.hp);
		 int curdecay = decay.Current(tracker.Average); // exibe o decay atual em hp/min (sliding window); nos primeiros 10s usa média dos combates anteriores
		 if (curdecay < 2000) tbdecay.Text = curdecay.ToString(); // exibe o decay atual no textbox 
		}


		// --------------------------------
		// ROTINAS ALL-CLASS - GCR
		// --------------------------------
		// NAO DEIXA AFOGAR 
		//------------------------------
		if (!dungeon) nao_afoga(); // nada para cima se estiver afogando; permite nadar em dungeons
		//-----------------------------------
		// ASSIST NO TANK EM DUNGEON 
		//-----------------------------------
		if (dungeon && cb_assist_tank.Checked && (!me.hastarget || tar.morreu))
		{
		 aperta(F6); // limpa o target e assiste o tank
		}
		// -----------------------------------------
		// BACKPEDAL SE APANHANDO NAS COSTAS 
		// -----------------------------------------
		if (me.mobs > 1 && (!dungeon || me.dazed)) // combate perigoso: múltiplos mobs (fora de dungeon ou com dazed)
		{
		 int limiar = int.Parse(tb_back_limiar.Text);      // valor-limite configurado pelo usuário
		 int decay = int.Parse(tbdecay.Text);              // dano por segundo atual
		 int avg_decay = int.Parse(tbavdecay.Text);        // média de decay

		 if (decay > 2000) decay = avg_decay;              // proteção contra leitura irreal

		 bool pode_backpedalar = false;

		 if (me.dazed)
		 {
			loga("Dazed! Backpedal para evitar mob batendo atrás.");
			pode_backpedalar = true;
		 }
		 else if (!ja_deu_backpedal && decay > limiar && tar.type != DRAGONKIN && !tar.casting && tar.hp > 20)
		 {
			loga($"Dando Backpedal: decay = {decay}");
			pode_backpedalar = true;
			ja_deu_backpedal = true;
		 }

		 if (pode_backpedalar)
		 {
			if (!cb_no_backpedal.Checked) aperta(SKEY, 1000);    // anda pra trás 1 segundo mantendo o facing
			if (!me.iscaster) aperta(AUTOATTACK);    // garante ataque ligado após reposicionamento
			else
			{
			 aperta(INTERACT); // se caster, apenas interage para manter o facing
			 loga("Interact code: 251"); // loga o código de interação
			}
		 }
		 else if (!jalogou)
		 {
			loga($"Backpedal não necessário: decay = {decay}");
			jalogou = true;
		 }
		}
		// -----------------------------------------
		// GIRA PARA O ALVO SE ESTIVER APANHANDO DE COSTAS
		// -----------------------------------------
		if (!dungeon && me.wrongway && cb_wrong_gira.Checked)
		{
		 aperta(INTERACT);
		 roda(20); // gira pra manter face no inimigo
		 loga("De costas para o alvo! Ativando correção.");
		 loga("Interact code: 252"); // loga o código de interação
																 // wait(2000); // espera 2s para não ficar girando muito
		 checkme(); // atualiza status após girar
		}

		// --------------------------------
		// REGISTRA MOB PARA SKIN
		// --------------------------------
		//wait(100);
		checkme();
		rastreia_mob_combate(); // registra o mob no tracking count  PARA LOOT/SKIN se necessário
		// --------------------------------
		// SAI SE FIM DE COMBATE
		// --------------------------------
			if (!me.combat) break; // combate terminou, sai do loop
// --------------------------------
// LIMPA ALVO INVALIDO OU MORTO
// --------------------------------

// Define se target tem aggro válido (em mim ou no pet)
checkme();
		if (tar.hp < 80) deucharge = false;
bool tem_aggro_valido = (tar.player_aggro || tar.pet_aggro);

// Define se deve preservar target mesmo sem aggro
bool deve_preservar = dungeon || deucharge;

// Limpa target SE: não tem aggro válido E não deve preservar

if (!tem_aggro_valido && !deve_preservar)
{
		 loga($"PET AGGRO: {tar.pet_aggro}, ME AGGRO: {tar.player_aggro}");
		 aperta(CLEARTGT);
    loga("Target sem aggro válido. Limpando.");
    checkme();
}
// Reseta flag do charge se ela estava ativa
else if (deucharge)
    deucharge = false; // permite limpar target nos próximos ciclos

// Target válido - continua o combate
if (tem_aggro_valido || deve_preservar)
{
    
// MANTEM  O TARGET 
}



		//-------------------------------------------------------------------
		//---------------------ROTINA EXCLUSIVA PALADINO --------------------
		//-------------------------------------------------------------------
		//-
		if (me.classe == PALADIN)           //  
		{
		 checkme();
		 getnear(); // se aproxima do target se estiver fora de alcance de melee
		 //wait_cast();                    // espera fim de cast se tiver algum
																		 // ------------------------------------------
																		 // MOVIMENTO: aproximação se fora de melee
																		 // ------------------------------------------
		 
		 if (tar.player_aggro  )
		 {
			aperta(INTERACT); // aproxima se fora de alcance
			loga("Apertando INTERACT para mob distante.");
		 }
		 if (me.hastarget && tar.player_aggro  && !me.autoattack)
			aperta(AUTOATTACK); // inicia ataque automático se tem target e não está atacando
		 if (tar.player_aggro  && me.wrongway)
		 {
			aperta(INTERACT); // gira para corrigir facing se necessário
			loga("Corrigindo facing com INTERACT. devido wrong way.");
			loga("Interact code: 253"); // loga o código de interação
		 }
		 if (false && tar.player_aggro && !(tar.type == HUMANOID && cb_nohumanoid.Checked))
		 {
			aperta(INTERACT);
			loga("apertando interact de rotina (agro > 0 e nao humanoid) no combate warrior - TESTE 0002 ");
		 }
		 //-----
		 if (tar.mood != 1 && !me.autoattack) // se mob não é amigável e não estou  atacando ele
			aperta(AUTOATTACK);         // inicia ataque automático
		 tenta_curar();                  // verifica se precisa curar e executa se necessário

		 // -------------------------------------
		 // EXORCISM - Usa em undead/demon se disponível e em alcance
		 // -------------------------------------
		 bool pode_exorcism = cb_use_exorcism.Checked
			&& me.level >= 20
			&& (tar.type == 150 || tar.type == 200)  // undead ou demon
			 && pala.exorcism_up
			 && mana(8);
		 if (pode_exorcism)
			aperta(EXORCISM);

		 //-----------------SEAL-------------------
		 aperta(best_seal()); // ativa o melhor seal SE NECESSARIO	

		 //----------------BLESSINGS----------------
		 bless(me);
		 // aplica blessing se necessário

		 //---------------STONEFORM------------------

		 //---------------AURAS------------------

		 // hp < 50% e quer Devotion Aura, mas não está ativa
		 if (!dungeon && me.hp < 50 && cb_devaura.Checked && !pala.devotion)
			aperta(DEVAURA, 1500); // ativa Devotion para defesa

		 // hp > 50% e quer Retribution Aura, mas não está ativa
		 else if (!dungeon && me.hp > 50 && cb_retaura.Checked && !pala.retribution)
			aperta(RETAURA, 1500); // ativa Retribution para dano
														 //--------------------------------------

		 // verifica se pode usar purify
		 int delay = atoi(tb_purify_delay); // lê delay desejado da toolbox (em segundos)
		 bool podepurify = Environment.TickCount - last_purify > delay * 1000; // verifica se passou tempo suficiente

		 if (cb_dwarf.Checked && me.racialready)                     // é anão e o racial está pronto
		 {
			int limiar = Convert.ToInt32(tb_stoneform_at.Text);     // lê o valor do limiar de HP do textbox
			if (me.hp < limiar || me.hasother || me.haspoison || me.hasdisease)
			 aperta(STONEFORM);                                // ativa Stoneform se atender condições
		 }
		 else if (me.level >= 8 // checa level
			 && cb_purify.Checked // checkbox ativo
			 && (me.haspoison || me.hasdisease) // tem debuff
			 && mana(20) // tem mana
			 && podepurify) // respeita o tempo mínimo entre usos
		 {
			aperta(PURIFY); // ativa o purify
			last_purify = Environment.TickCount; // atualiza o tempo do último uso
		 }

		 if (should_judge())                                         // se condições pro Judgement estão ok
		 {
			aperta(INTERACT);                                 // garante que está virado para o mob
			aperta(PULA);                                           // pula antes de atacar (visual agressivo)
			aperta(JUDGEMENT);                                      // executa o Judgement
		 }

		 if (should_stun())                                          // verifica se deve usar stun
			aperta(HOJ);                                            // aplica Hammer of Justice

		 
		}

		//----------------------------------
		// ROTINA DE COMBATE HUNTER (COPIADA DO ROGUE)
		//----------------------------------
		else if (me.classe == HUNTER)
		{
		 // ------------------------------------------
		 // MOVIMENTO: aproximação se fora de melee
		 // ------------------------------------------
		 checkme();

		 getnear();

		 // Aproximação (COPIADO DO GETNEAR PARA MELEE CLASSES)
		 if (me.wrongway)
		 {
			loga("Hunter: Corrigindo wrongway (code 302).");
			aperta(INTERACT);
		 }
		 if (me.hastarget && (tar.pet_aggro || tar.player_aggro))
		 {
			if (!me.melee)
			 loga("Hunter: Corrigindo distância (fora de melee) (code 303).");
			else
			 loga("Interact de rotina: 305."); // loga o código de interação
			aperta(INTERACT);
		 }

		 // ASSIST NO PET 
		 //------------------------------
		 if ((!me.hastarget && hunt.has_pet && me.combat) || (me.mobs > 1 && hunt.has_pet))
			aperta(ASSIST); // assiste o pet se estiver em combate sem target ou +1 mob

		 // ------------------------------------------
		 // AUTOATTACK + PULOS
		 // ------------------------------------------
		 if (me.hastarget && tar.player_aggro && tar.mood != 1 && !me.autoattack)
		 {
			aperta(AUTOATTACK);
			loga("Autoattack iniciado.");
		 }

		 if (ticker % 6 == 0) aperta(PULA); // pulo human-like

		 // ------------------------------------------
		 // EMERGÊNCIA: POÇÃO SE MORRENDO
		 // ------------------------------------------
		 if (me.hp < 30 && me.hp_potion_rdy)
		 {
			aperta(HEALTHPOTION);
			clog($"Emergência! HP: {me.hp}%");
		 }

		 // ------------------------------------------
		 // STONEFORM (COPIADO DO PALADINO)
		 // ------------------------------------------
		 else if (cb_dwarf.Checked && me.racialready)                     // é anão e o racial está pronto
		 {
			int limiar = Convert.ToInt32(tb_stoneform_at.Text);          // lê o valor do limiar de HP do textbox
			if (me.hp < limiar || me.hasother || me.haspoison || me.hasdisease)
			{
			 aperta(STONEFORM);                                       // ativa Stoneform se atender condições
			 clog($"Hunter: Stoneform usado! HP: {me.hp}% Debuffs: {me.hasother}/{me.haspoison}/{me.hasdisease}");
			}
		 }
		 // ------------------------------------------
		 // RANGED TRY
		 // ------------------------------------------
		 checkme();
		 if (hunt.serpent_sting_up && !me.melee)
		 {
			aperta(SERPENTSTING); // (comentado: ainda não aprendeu)
			clog("Casting Serpent Sting.");
		 }
		 else if (hunt.arcane_shot_up && !me.melee)
		 {
			aperta(ARCANESHOT); // (comentado: ainda não aprendeu)
			clog("Casting Arcane Shot.");
		 }
		 // ------------------------------------------
		 // RAPTOR STRIKE (SUBSTITUI TODAS AS SKILLS DO ROGUE)
		 // ------------------------------------------
		 else if (hunt.raptor_strike_up && tar.hp > 0)
		 {
			checkme();
			if (hunt.raptor_strike_up) aperta(RAPTORS);
			clog($"Raptor Strike: mana={me.mana}");
		 }



		}//------------FIM ROTINA DE COMBATE HUNTER ------------------

		// -----------------------------------------------
		// ROTINA DE COMBATE WARRIOR (WCR)
		// -----------------------------------------------
		else if (me.classe == WARRIOR)
		{
		 // ------------------------------------------
		 // AUTOATTACK + PULOS
		 // ------------------------------------------
		 if (tar.mood != 1 && !me.autoattack) aperta(AUTOATTACK); // garante autoattack se mob hostil
		 if (ticker % 6 == 0 && !me.casting) aperta(PULA);                        // pulo human-like

		 // -----------------------------------------------
		 // DEFESA: RETALIATION E POTION
		 // -----------------------------------------------
		 if ((!me.hp_potion_rdy && war.retaliation_up) && // sem potion e skill pronta
			 	((me.hp < 50 && me.mobs >= 2) ||   // vida < 50 e 2+ mobs
				 (me.hp < 65 && me.mobs >= 3) ||   // vida < 65 e 3+ mobs
				 (me.hp < 45 && me.mobs == 1)))      // vida < 30 com 1 mob
			 casta(RETALIATION);                  // aciona Retaliation


		 if (war.overpower_up)
			aperta(OVERPOWER); // usa Overpower se disponível

		 if (me.hp < 30 && me.hp_potion_rdy)
			aperta(HEALTHPOTION); // poção se vida muito baixa



		 // ------------------------------------------
		 // EXECUTE
		 // ------------------------------------------
		 else if (tar.hp > 0 && war.execute_up && tar.hp <= 20)
		 {
			//clog($"Slam: {war.slam_up}");
			casta(EXECUTE);
		 }
		 // ------------------------------------------
		 // MOVIMENTO: aproximação se fora de melee
		 // ------------------------------------------
		 getnear(); // se aproxima do target se estiver fora de alcance de melee
		 if (tar.player_aggro  && !me.melee)
		 {
			aperta(INTERACT); // aproxima se fora de alcance
			loga("Apertando INTERACT para mob distante.");
		 }
		 if (me.hastarget && tar.player_aggro  && !me.autoattack)
			aperta(AUTOATTACK); // inicia ataque automático se tem target e não está atacando
		 if (tar.player_aggro  && me.wrongway)
		 {
			aperta(INTERACT); // gira para corrigir facing se necessário
			loga("Corrigindo facing com INTERACT. devido wrong way.");
			loga("Interact code: 253"); // loga o código de interação
		 }
		 if (false && tar.player_aggro && !(tar.type == HUMANOID && cb_nohumanoid.Checked))
		 {
			aperta(INTERACT);
			loga("apertando interact de rotina (agro > 0 e nao humanoid) no combate warrior - TESTE 0002 ");
		 }

		 // ------------------------------------------
		 // BATTLE SHOUT  (sempre)=10 rage
		 // ------------------------------------------
		 else if (war.bs_up && !war.has_bs)
		 {
			aperta(BATTLESHOUT);
			clog("Battle Shout.");
			
		 }
		 // ------------------------------------------
		 // EXECUTE
		 // ------------------------------------------
		 else if (tar.hp > 0 && war.execute_up && tar.hp <= 20)
			casta(EXECUTE);
		 // ------------------------------------------
		 // DEMORALIZING SHOUT  
		 // ------------------------------------------
		 else if (
				 cb_use_demoshout.Checked &&                                      // checkbox ativado
				 me.mobs >= atoi(tb_demoshoutat) &&                          // número de mobs >= valor do textbox
				 ((!tar.trivial || me.mobs > 1) && war.demo_up && !war.has_demoralizing) // condições do Demo Shout
		 )
		 {
			aperta(DEMORALIZING); // reduz dano dos mobs
			clog("Demoralizing Shout.");
			
		 }

		 // ------------------------------------------
		 // HAMSTRING se humanoide e fugindo = 10 rage
		 // ------------------------------------------
		 else if (war.hams_up && (tar.type == MURLOC || (tar.type == HUMANOID && tar.hp < 35)))
		 {
			aperta(HAMSTRING); // impede fuga do mob 
			clog($"Hamstring: humanoid fugindo com {tar.hp}% HP");
		 }
		 // ------------------------------------------
		 // THUNDER CLAP  (multi-target) = 20 rage
		 // ------------------------------------------
		 else if (cb_use_thunderclap.Checked && war.thun_up      // checkbox ativa, thunderclap pronto e com rage
			&& !war.has_thunderclap                                 // target ainda não tem debuff
			&& me.mobs >= atoi(tb_thunderclap_count))               // tem quantidade mínima de mobs
		 {
			aperta(THUNDERCLAP);
			clog($"Thunderclap: mobs={me.mobs}");
		 }
		 // ------------------------------------------
		 // CLEAVE se mais de 1 mob
		 // ------------------------------------------
		 else if (war.cleave_up && me.mobs > 1 && !war.has_cleave)
		 {
			aperta(CLEAVE); // cleave para hits em área
			clog($"Cleave: mobs={me.mobs}");
			
		 }
		 // ------------------------------------------
		 // SLAM sempre que possivel e nao for castar sunder armor
		 // ------------------------------------------
		 else if (cb_slam.Checked && tar.hp >= 20 && me.mobs == 1 && war.slam_up && (tar.hp <= 60 || !cb_sunderspam.Checked))
		 {
			
			casta(SLAM);
			clog("Slam!");
		 }

		 // ------------------------------------------
		 // SUNDER ARMOR SPAM
		 // ------------------------------------------
		 else if (tar.hp > (cb_slam.Checked ? 60 : 40) && war.sunder_up && cb_sunderspam.Checked)
		 {
			aperta(SUNDERARMOR);
			clog("Sunder armor.");
			
		 }
		 // ------------------------------------------
		 // APLICA REND SE PERMITIDO E EFICAZ= 10 rage
		 // ------------------------------------------
		 else if (me.mobs < 2 && cb_use_rend.Checked && war.rend_up && !war.has_rend
	&& (tar.type == HUMANOID                               // sempre aplica em humanóide, pra evitar fugir. 
		 || (
			 (!tar.trivial
			 && tar.type != ELEMENTAL                           // ignora elementais
			 && tar.type != MECHANICAL                          // ignora mecânicos
			 && tar.type != UNDEAD                              // ignora mortos-vivos
		 ))))
		 {
			casta(REND);
			clog($"Rend: aplicado em {tar.type} com {tar.hp}% HP  Trivial: {tar.trivial}");
		 }

		 // ------------------------------------------
		 // HEROIC STRIKE  fallback (rage dump)
		 // ------------------------------------------
		 else if (war.hs_up && !war.hs_casting && me.mana >= atoi(tb_heroic_strike_rage) &&
							// nao ativado sunder spam ou rage em excesso (rage dump)
							!(tar.hp > 0 && tar.hp < 30 && war.execute_up)) // NÃO usa HS se mob < 28% HP e Execute disponível
		 {
			aperta(HEROICS);
			clog($"Heroic Strike: rage={me.mana}");
		 }

		 else if ((!tar.trivial || me.mobs > 1) && me.hp > 50 && cb_use_bloodrage.Checked && me.mana < 15 && war.bloodrage_up)
		 {
			aperta(BLOODRAGE); // ativa Bloodrage se vida > 50, rage baixa e (target não trivial OU múltiplos mobs)
			clog("Bloodrage.");
		 }


		 //-------------------AGUARDANDO IMPLEMENTAÇÃO-------------------







		}//------------FIM ROTINA DE COMBATE WARRIOR (WCR) ------------------


		//----------------------------------
		// ROTINA DE COMBATE WARLOCK
		//----------------------------------

		else if (me.classe == WARLOCK)
		{
		 void castacurse()
		 {
			if (wlock.has_curse_agony || wlock.has_curse_weakness) return;

			if (cb_autocurse.Checked)
			{
			 if (!tar.trivial) casta(CURSEWEAKNESS);
			 else
					 if (tar.hp > 25) casta(CURSEAGONY);
			}
			else if (cb_COW.Checked && wlock.curse_weakness_up && !wlock.has_curse_weakness)
			{
			 loga("Aplicando Curse of Weakness.");
			 casta(CURSEWEAKNESS);
			}
			else if (cb_COA.Checked && wlock.curse_agony_up && !wlock.has_curse_agony)
			{
			 loga("Aplicando Curse of Agony.");
			 casta(CURSEAGONY);
			}
		 }

		 //---------------------------
		 // FACE NO TARGET 
		 //------------------------------
		 getnear(false); // se aproxima do target se estiver fora de alcance de melee
		 if (me.wrongway && (tar.player_aggro  || tar.pet_aggro ))
		 {
			casta(INTERACT); // gira para corrigir facing se necessário
			loga("Interact code: 254"); // loga o código de interação
		 }

		 // ASSIST NO PET 
		 //------------------------------
		 if ((!me.hastarget && wlock.has_pet && me.combat) || (me.mobs > 1 && wlock.has_pet))
			aperta(ASSIST); // assiste o pet se estiver em combate sem target ou +1 mob

		 // MANTEM BUFFS (demon skin)
		 //------------------------------
		 if (!wlock.has_demon_skin)
		 {
			loga("Ativando Demon Skin.");
			casta(DEMONSKIN);
			checkme();
		 }

		 // ================================
		 // EMERGÊNCIA: HEALTHSTONE OU POÇÃO
		 // ================================
		 if (me.hp < 40)
		 {
			// Usa Healthstone primeiro, se tiver e pronta
			if (wlock.healthstone_up)
			{
			 loga("HP crítico! Usando Healthstone.");
			 aperta(HEALTHSTONE);
			 checkme();
			}
			// Se não tem Healthstone ou cooldown, tenta potion
			else if (me.hp < 30 && me.hp_potion_rdy)
			{
			 loga("HP crítico! Usando poção.");
			 aperta(HEALTHPOTION);
			 wait(500);
			}
		 }


		 // ================================
		 // CURA DO PET (HEALTH FUNNEL)
		 // ================================
		 if (me.hp > 60 && wlock.healhfunnel_up && wlock.pet_hp > 0 && wlock.pet_hp < 60)
		 {
			loga("Pet com HP baixo. Usando Health Funnel.");
			casta(HEALTHFUNNEL);
			do
			{
			 checkme();
			 wait(1000); // espera 1 segundo para não spam
			} while (me.casting && me.hp > 60 && wlock.pet_hp < 90);
			// espera terminar o cast
		 }

		 // ================================
		 // SACRIFÍCIO DO PET (EMERGÊNCIA)
		 // ================================
		 if (me.hp < 10 && me.level >= 16 && wlock.has_pet)
		 {
			loga("HP crítico! Sacrificando Voidwalker.");
			// Implementar sacrifício do pet quando necessário
		 }

		 // ================================
		 // APLICAR DEBUFFS (PRIORIDADE MÁXIMA)
		 // ================================
		 if (!me.casting) castacurse(); // mas nao vai parar o drain...
		 checkme();

		 // Corruption (se não tiver)
		 if (tar.hp > 15 && cb_use_corruption.Checked && wlock.corruption_up && !wlock.has_corruption)
		 {
			loga("Aplicando Corruption.");
			castslow(CORRUPTION);
			viramob();
		 }
		 // IMMOLATE
		 else if (me.mana > 30 && tar.hp > 40 && (me.mobs < 2 || (me.mobs == 2 && wlock.has_pet)) && cb_use_immolate.Checked && wlock.immolate_up && !wlock.has_immolate)
		 {
			loga($"Status Immolate: immolate_up={wlock.immolate_up} / has_immolate={wlock.has_immolate}");
			loga("Aplicando Immolate.");
			viramob();
			casta(IMMOLATE);
		 }
		 // Siphon Life (se não tiver)
		 else if (wlock.siphon_life_up && !wlock.has_siphon_life)
		 {
			loga("Aplicando Siphon Life.");
			aperta(SIPHONLIFE);
		 }
		 // ================================
		 // CHANNELING (PRIORIDADE ALTA)
		 // ================================
		 else if (!me.melee) // Debuffs castados, sem melee range
		 {
			aperta(INTERACT); // garante que está virado para o target
			clog("Interact por !me.melee. Cod. 001");
		 }

		 // Drain Life para cura (como antes)
		 else if (me.hp < 70 && wlock.drain_life_up && !wlock.has_drain_life)
		 {
			clog("HP baixo. Usando Drain Life.");
			castslow(DRAINLIFE);
		 }

		 // Substitui uso ofensivo do Drain Soul por Drain Life se não precisar de shard
		 else if (tar.hp < 40 && wlock.drain_life_up && !wlock.has_drain_life && wlock.shards >= 2)
		 {
			if (me.wandon) stopwand(); // Interrompe wand para canalizar
			clog("Mob morrendo e já tenho shards. Usando Drain Life.");
			castslow(DRAINLIFE);
		 }

		 // Drain Soul real – só se estiver precisando de shard
		 else if ((tar.hp < 40 || !cb_wand.Checked) && wlock.drain_soul_up && !wlock.has_drain_soul && wlock.shards < 2)
		 {
			if (me.wandon && tar.hp < 40) stopwand(); // Para wand se necessário
			clog("Shard abaixo de 2. Usando Drain Soul.");
			casta(DRAINSOUL);
		 }



		 // ================================
		 // FILLER: SHADOW BOLT (SPAM)
		 // ================================
		 // Shadow Bolt (filler principal)
		 else if (cb_use_shadowbolt.Checked && me.mana > atoi(tb_shadowbolt_mana) && wlock.shadowbolt_up && me.mobs < 2)
		 {
			viramob(); // garante que está virado para o target
			castslow(SHADOWBOLT);
		 }

		 // ================================
		 // FALLBACK: AUTOATTACK ou WAND
		 // ================================
		 else
		 {
			// Usa wand se Drain Soul não estiver disponível E wand estiver disponível
			if (!wlock.drain_soul_up && me.wand_up && !me.wandon && cb_wand.Checked && !me.casting)
			{
			 viramob();
			 aperta(WAND, 500); // garante que está com wand ligada 
			 aperta(STOPCAST); // uma wandada só 
			}
		 }
		 if (tar.hp < 35 && me.wandon &&
		((tar.hp < 30 || !cb_wand.Checked) && !wlock.has_drain_soul &&
		 (wlock.shards < 3 || cb_drain_soul.Checked)))
		 {
			loga("Parando wand: Target baixo e Drain Soul será necessário.");
			stopwand();
			checkme(); // atualiza status após parar wand
		 }

		}//------------FIM ROTINA DE COMBATE WARLOCK ------------------
		 // ----------------------------------
		 // ROTINA DE COMBATE PRIEST
		 // ----------------------------------
		else if (me.classe == PRIEST)
		{
		 // Virar para o target se estiver de costas
		 if (me.wrongway)
			viramob();

		 // POWER WORD: SHIELD
		 if (cb_combat_pws.Checked && !priest.has_pws && !priest.has_weakened_soul && priest.pws_up &&
			(me.mobs > 1 || (me.mobs == 1 && (me.hp < 20 || (me.hp >= 20 && tar.hp > 25)))))
		 {
			stopwand();
			casta(PWS);
			clog("Combat: Power Word: Shield");
		 }
		 // HEAL CRÍTICO
		 if (me.mobs > 1 && me.hp < 50 && priest.desperate_prayer_up)
		 {
			stopwand();
			casta(DESPERATE); // DESPERATE PRAYER 
		 }
		 else if (me.mobs > 2 && me.hp < 50 && !priest.desperate_prayer_up && me.hp_potion_rdy)
		 {
			aperta(HEALTHPOTION); // POÇÃO DE VIDA
		 }
		 // HEAL (COMBAT HEAL)
		 else if (me.hp < atoi(tb_priest_combatheal) && priest.heal_up)
			{
			 stopwand();
			 casta(HEAL);
			 clog($"Combat: Heal crítico - HP: {me.hp}%");
			}
			// RENEW
			else if (
				(
					(me.hp < atoi(tb_renewat) && !priest.has_renew && priest.renew_up)
					||
					(!priest.has_pws && priest.has_weakened_soul && !priest.has_renew && priest.renew_up)
				)
				&&
				me.hp <= 85
				&&
				!(me.mobs == 1 && me.hp > 60 && tar.hp < 25)
				)
			{
			 stopwand();
			 casta(RENEW);
			 clog($"Combat: Renew - HP: {me.hp}%");
			}


			// FORTITUDE
			else if (!priest.has_fortitude && priest.fortitude_up)
			{
			 stopwand();
			 casta(FORTITUDE);
			 clog("Combat: Fortitude");
			}
			// SHADOW WORD: PAIN
			else if (!priest.has_swp && tar.hp > 15 && priest.swp_up)
			{
			 viramob();
			 stopwand();
			 casta(SWP);
			 clog("Combat: Shadow Word: Pain");
			}
			// SMITE
			else if (cb_usesmite.Checked && me.mana > atoi(tb_smitemana) &&
					tar.hp > 20 && (!cb_shielded_smite.Checked || priest.has_pws) && priest.smite_up)
			{
			 viramob();
			 stopwand();
			 casta(SMITE);
			 clog("Combat: Smite");
			}
			// ================================
			// FALLBACK: AUTOATTACK ou WAND
			// ================================
			else
			{
			 // Se não pode fazer nada, garante wand ativa
			 if (!me.wandon && cb_use_priest_wand.Checked && !me.casting)
			 {
				viramob();
				aperta(WAND, 500); // garante que está com wand ligada
			 }
			}
		 }
		 //------------FIM ROTINA DE COMBATE PRIEST ------------------



		 // -----------------------------------------------
		 // ROTINA DE COMBATE ROGUE (RCR)
		 // -----------------------------------------------
		 else if (me.classe == ROGUE)
		 {
			// ------------------------------------------
			// MOVIMENTO: aproximação se fora de melee
			// ------------------------------------------
			checkme();
			loga($"Aggro: {tar.player_aggro} Ticker: {ticker}"); // loga o ticker atual
			if (tar.player_aggro ) aperta(INTERACT); // gira para corrigir facing se necessário
																					 //getnear();// Aproxima do target se necessário
																					 // ------------------------------------------
																					 // AUTOATTACK + PULOS
																					 // ------------------------------------------
			if (me.hastarget && tar.player_aggro  && tar.mood != 1 && !me.autoattack)
			{
			 aperta(AUTOATTACK);
			 loga("Autoattack iniciado. Cod 323");
			}
			if (ticker % 3 == 0) aperta(PULA);                        // human-like pulo eventual
																																// -----------------------------------------------------------------------
																																// VANISH 
																																// -----------------------------------------------------------------------
			if (me.mobs == 3)// TESTE 
			{
			 if (rog.vanish_up)
			 {
				casta(STOPATTACK); // para auto attack 
				checkme();
				if (rog.gouge_up) casta(GOUGE); // tenta Gouge para imobilizar mobs se disponível
				checkme();
				aperta(CLEARTGT); // limpa o target
				aperta(VANISH);
				aperta(CLEARTGT); // limpa o target
				anda(-6); // recua 6 metros  para evitar aggro imediato
				espera(2);
				checkme();
				if (rog.stealth_up) casta(STEALTH);
				while (me.hp < 100 && !me.combat)
				{
				 espera(1); // espera 10 segundos para evitar spam de Vanish

				}
			 }

			}
			// -----------------------------------------------------------------------
			// EVASION: Se o HP estiver abaixo do limiar configurado,
			// -----------------------------------------------------------------------
			if (me.hp < atoi(tb_evasion))
			{
			 if (rog.evasion_up)                                      // se Evasion está pronta
			 {
				if (!(me.mobs == 1 && tar.hp <= 25))                // se NÃO é apenas 1 mob com 25% ou menos de vida
				 aperta(EVASION);                               // então usa Evasion
				clog($"Combat: Evasion - HP: {me.hp}%"); // loga o uso de Evasion
			 }
			}
			if (me.hp < 35 && me.hp_potion_rdy)
			{
			 aperta(HEALTHPOTION); // usa poção de cura se HP < 35% e poção pronta
			 clog($"Combat: Health Potion - HP: {me.hp}%"); // loga o uso da poção
			}


			// ------------------------------------------
			// KICK
			// ------------------------------------------

			if (tar.castbar > 0)
			{
			 loga("Tentando interromper cast do mob.");
			 loga(rog.kick_up.ToString());
			 if (rog.kick_up)
				aperta(KICK, 1000); // interrompe cast do mob se possível
			}

			// ------------------------------------------
			// COMBATE SEM COMBO POINTS
			// ------------------------------------------
			if (rog.combo == 0)
			{
			 if (rog.ss_up)
			 {
				{ // sinister strike com log 
				 int preen = me.mana;
				 int prehp = tar.hp;
				 aperta(SS, 1000);  // seu código existente
				 checkme();
				 int dano_ss = prehp - tar.hp;
				 int managasta = preen - me.mana; // energia gasta pelo SS
																					// Registra energia e dano do SS
				if (managasta > 0)
				 ComboOptimizer.RegistrarCast(
						 atoi(tb_energy_ss),
						 (int)Math.Round(dano_ss * ((double)atoi(tb_damage_ss) - (double)atoi(tb_damage_hit)) / (double)atoi(tb_damage_ss))
				 );
				clog($"Sinister Strike: CP: {rog.combo} HP: {tar.hp}% Damage: {prehp - tar.hp}"); // loga o uso de Sinister Strike
				}
			 }
			}
			// ------------------------------------------
			// COMBATE COM COMBO POINTS
			// ------------------------------------------
			else
			{
			 // ------------------------------------------
			 // EVISCERATE
			 // ------------------------------------------			
			 if (cb_evis_auto.Checked)
			 {
				int dif = tar.level - me.level; // diferença entre os níveis
				int pontos;
				if (dif <= -2) // mob 2 ou mais níveis abaixo
				 pontos = 2;
				else if (dif == -1) // mob 1 nível abaixo
				 pontos = 3;
				else if (dif == 0) // mesmo nível
				 pontos = 4;
				else // mob acima do meu nível
				 pontos = 5;

				// APLICA A VARIAÇÃO BASEADA NA CATEGORIA ATUAL
				pontos += ComboOptimizer.categoria_atual;

				// Garante limites mínimos e máximos
				pontos = Math.Max(1, Math.Min(5, pontos));

				tb_evis_cp.Text = pontos.ToString(); // mostra no textbox
			 }

			 bool finalizavel = tar.hp <= 30;
			 bool rotina = rog.combo >= atoi(tb_evis_cp);  // combo ideal
			 bool pode_evis = rog.evis_up && (finalizavel || rotina);

			 if (pode_evis)
			 {
				checkme();
				int prehp = tar.hp;
				int premana = me.mana;

				aperta(PULA); // pula antes de atacar (visual agressivo)
				aperta(EVIS, 1000);  // mob vai morrer ou combo cheio
				checkme();


				int dano = prehp - tar.hp;
				int poscombo = rog.combo; // salva o combo após o ataque
																	// Registra energia e dano do eviscerate
				if (me.mana < premana)
				{
				 ComboOptimizer.RegistrarCast(35, dano);

				 clog($"Eviscerate: CP: {rog.combo} HP: {tar.hp}% Damage: {dano} Category: {ComboOptimizer.categoria_atual}");
				}
				else clog("Eviscerate cast fail.");
			 }

			 // ------------------------------------------
			 // SLICE AND DICE
			 // ------------------------------------------		
			 else if (!(me.mobs == 1 && tar.trivial) && !rog.has_SAD && rog.SAD_up && cb_SAD.Checked)
			 {
				casta(SAD);   // não tem Slice and Dice → aplica se indicado
			 }
			 // ------------------------------------------
			 // EXPOSE ARMOR
			 // ------------------------------------------					
			 else if (!tar.trivial && (rog.has_SAD || !cb_SAD.Checked) && rog.expose_armor_up && !rog.has_expose_armor && tar.hp > 70
				 && cb_expose_armor.Checked)

			 {
				clog("Aplicando Expose Armor.");
				aperta(EXPOSE_ARMOR, 1000);
				checkme();
			 }

			 // ------------------------------------------
			 // SINISTER STRIKE (energy dump)
			 // ------------------------------------------						
			 else if (rog.ss_up && mana(45))
			 {
				checkme();
				int prehp = tar.hp; // salva o HP antes do ataque
				int precombo = me.mana; // salva o combo antes do ataque
				if (rog.ss_up)
				{
				 prehp = tar.hp;
				 aperta(SS, 1000);  // seu código existente
				 checkme();
				 int dano_ss = prehp - tar.hp;
				 int poscombo = me.mana; // salva o combo após o ataque
																 // Registra energia e dano do SS
				 if (poscombo < precombo)
				 {

				 ComboOptimizer.RegistrarCast(
						 atoi(tb_energy_ss),
						 (int)Math.Round(dano_ss * ((double)atoi(tb_damage_ss) - (double)atoi(tb_damage_hit)) / (double)atoi(tb_damage_ss))
				 );
				 clog($"Sinister Strike: CP: {rog.combo} HP: {tar.hp}% Damage: {prehp - tar.hp}"); // loga o uso de Sinister Strike

				}
				else clog("Sinister Strike cast fail.");
				}
				checkme();

			 }

			}
		 }
		checkme();

	 } while (me.combat); // FIM DO LOOP DE COMBATE 
	 aperta(STOPATTACK); // para o auto-ataque no final do combate
											 //---------------------------------------------
											 // TERMINA O COMBATE 
											 // ---------------------------------------------
	 clog($"Combate encerrado. Ciclos: {combat_ticker}");
	 killstats("save"); // atualiza stats de kills totais 
	 // PALADINO (reseta variaveis de combate) 
	 //----------------------------------------------
	 pala.defseal = false;     // volta a permitir uso de SOR
	 pala.defbless = false;    // libera BOK ou BOM de novo
	 pala.defaura = false;     // pode voltar pra Ret Aura
	 pala.nomana = false;      // desativa fixação de BOW
														 //----------------------------------------------------
														 // ---------------- ROTINAS PÓS COMBATE GENERICAS
														 // ---- decay --- 
	 int final = decay.End(me.hp);
	 tracker.Add(final);
	 tbavdecay.Text = tracker.Average.ToString();
	 tbdecay.Text = final.ToString(); // opcional: mostrar o valor final após a luta
	 emCombate = false;
	 //------Timer de kills 
	 last_kill_time = Environment.TickCount;
	 //-----------------------------------------
	 // ROTINA PÓS-COMBATE (ROGUE)
	 //-----------------------------------------
	 if (me.classe == ROGUE)
	 {
		if (cb_randomize_rogue.Checked) //  RANDOMIZA TIPO DE PULL
		{
		 Random random = new Random();

		 // 50% de chance para cb_range_pull
		 cb_range_pull.Checked = random.Next(0, 2) == 1;

		 // 50% de chance para cb_stealth_pull
		 cb_stealth_pull.Checked = random.Next(0, 2) == 1;
		}
	 }

	 // ================================
	 // ROTINA PÓS COMBATE - HUNTER (ATUALIZADA)
	 // Adicionar esta seção na rotina pós combate do Hunter, após o código de recuperação de HP
	 // ================================
	 else if (me.classe == HUNTER)
	 {
		// ================================
		// LÓGICA INTELIGENTE DE PET
		// ================================
		if (me.level >= 10) // Hunter ganha pet no level 10
		{
		 // CASO 1: Pet morto (has_pet = true mas pet_hp = 0)
		 if (!hunt.has_pet  && hunt.revive_pet_up)
		 {
			loga("Pet morto detectado. Revivendo pet.");
			para(); // para de andar para reviver

			// Aguarda um momento para garantir que parou
			wait(500);

			// Verifica se tem mana suficiente (Revive Pet custa mana)
			if (me.mana < 50)
			{
			 loga("Mana insuficiente para reviver pet. Esperando recuperação.");
			 aperta(F12); // COMIDA/BEBIDA para recuperar mana
			 while (me.mana < 50 && !me.combat)
			 {
				wait(1000);
				checkme();
			 }
			}

			// Revive o pet
			loga("Revivendo pet...");
			casta(SUMMONPET); // usa o método casta para reviver o pet

			// Aguarda um pouco para o pet ser revivido
			wait(3000);
			checkme(); // atualiza status para verificar se o pet foi revivido

			if (hunt.has_pet && hunt.pet_hp > 0)
			{
			 loga("Pet revivido com sucesso!");
			}
			else
			{
			 loga("Falha ao reviver pet. Tentativa será repetida no próximo ciclo.");
			}
		 }
		 // CASO 2: Sem pet (has_pet = false) - não faz nada
		 else if (!hunt.has_pet && !hunt.revive_pet_up)
		 {
			loga("Hunter sem pet. Precisa fazer tame primeiro.");
			// Não tenta invocar nada - precisa fazer tame de um animal
		 }
		 // CASO 3: Pet vivo (has_pet = true e pet_hp > 0) - tudo ok
		 else if (hunt.has_pet && hunt.pet_hp > 0) // ALIMENTA O PET
		 {
			// X = mesmo do botão de apagar cinza (5)
			// Y = botão de apagar cinza (5) + 10 pixels = 15
			loc botao_feed_pet = new loc(5, 15);

			loga("Alimentando pet...");
			clica(botao_feed_pet, 1); // clica no botão de alimentar pet existente
			wait(50); // pequena pausa para evitar spam
		 }
		}

	
		// ================================
		// RECUPERAÇÃO DE HP (código existente)
		// ================================
		int hp_ini = Math.Min(me.hp,hunt.pet_hp);                              // salva o HP inicial
		int limite = 80;                                 // Hunter usa mesmo limite do Warrior (80%)

		if (hp_ini < limite)
		{
		 loga($"HUNTER: HP baixo após combate ({Math.Min(me.hp, hunt.pet_hp)}%). Esperando recuperação.");
		 para(); // para de andar se estiver andando
		 loga($"Esperando recuperação de HP: {Math.Min(me.hp, hunt.pet_hp)}");
		 aperta(F12); // COMIDA 
		 espera(2);

		 // se começou a comer, espera até 100%; senão, só até o limite normal
		 if (me.eating)
		 {
			while (Math.Min(me.hp, hunt.pet_hp) < 100 && !me.combat)
			 espera(1);
		 }
		 else
		 {
			while (Math.Min(me.hp, hunt.pet_hp) < limite && !me.combat)
			 espera(1);
		 }
		}
	 }//-------------FIM DA ROTINA DE PÓS COMBATE HUNTER ------------------

	 //-----------------------------------------
	 // ROTINA DE DESCANSO PÓS-COMBATE (WARRIOR)
	 //-----------------------------------------
	 else if (me.classe == WARRIOR)
	 {
		//aperta(CLEARTGT); // limpa o target após o combate
		if (cb_random_pull_warrior.Checked) //  RANDOMIZA TIPO DE PULL (Charge / ranged)
		{
		 Random random = new Random();

		 // 50% de chance para cb_range_pull
		 cb_war_rangepull.Checked = random.Next(0, 2) == 1;

		}

		int hp_ini = me.hp;                              // salva o HP inicial
		int limite = atoi(tb_rest_warr);                 // valor de referência para descanso

		if (hp_ini < limite)
		{
		 loga($"WARRIOR: HP baixo após combate ({hp_ini}%). Esperando recuperação.");
		 aperta(F12); // ativa comida
		 espera(2);
		 // se começou a comer, espera até 100%; senão, só até o limite normal
		 if (me.eating)
		 {
			while (me.hp < 100 && !me.combat)
			 espera(1);
		 }
		 else
		 {
			while (me.hp < limite && !me.combat)
			 espera(1);
		 }
		}
	 }// FIM ROTINA DE DESCANSO PÓS-COMBATE (WARRIOR)
		//----------------------------------
		// RECUPERAÇÃO E PREPARO PRÉ COMBATE - PRIEST
		//----------------------------------
	 else if (me.classe == PRIEST)
	 {
		// Cura se vida baixa
		if (me.hp < atoi(tb_priest_pullheal))
		{
		 para(); // para de andar se estiver andando
		 loga($"Pós combate: Curando - HP: {me.hp}%");

		 // Aplicar PWS se não tem e não tem weakened soul
		 if (false && !priest.has_pws && !priest.has_weakened_soul && priest.pws_up)
		 {
			casta(PWS);
			
		 }

		 // Aplicar Renew se não tem
		 if (!priest.has_renew && priest.renew_up)
		 {
			casta(RENEW);
			
		 }

		 // Heal direto se muito baixo
		 if (me.hp < atoi(tb_priest_combatheal) && priest.heal_up)
		 {
			casta(HEAL);
			
		 }
		}

		// Mana baixa - comer/beber
		if (me.mana < atoi(tb_mana_pull_priest))
		{
		 para(); // para de andar se estiver andando
		 clog($"Priest: Recuperando mana: {me.mana}%");
		 aperta(F12); // COMIDA/BEBIDA
		 while (me.mana < atoi(tb_mana_pull_priest) && !me.combat)
		 {
			espera(1);
			checkme();
		 }
		}

		// Aplicar buffs se não tem
		if (!priest.has_fortitude && priest.fortitude_up)
		{
		clog("Priest: Aplicando Power Word: Fortitude");
		 casta(FORTITUDE);
		 
		}

		if (!priest.has_innerfire && priest.innerfire_up)
		{
		 clog("Priest: Aplicando Inner Fire");
		 casta(INNERFIRE);
		 
		}
	 }

	}
	// --------------------------------
	// APROXIMA-SE DO TARGET (OU VIRA)
	// --------------------------------
	void getnear(bool melee = true)
	{
	 if (me.classe == HUNTER) return; // não aproxima em hunter

	 if (dungeon) return; // não aproxima em dungeons

	 // -----------------------
	 // Atualiza estado do player
	 // -----------------------
	 checkme();

	 // -----------------------
	 // Se não há target ou aggro, não faz nada
	 // -----------------------
	 if (!me.hastarget || (!tar.player_aggro  && !tar.pet_aggro))
	 {
		loga("getnear: Sem target ou sem aggro, retornando.");
		return;
	 }

	 // -----------------------
	 // MELEE CLASSES
	 // -----------------------
	 if (me.classe == WARRIOR || me.classe == ROGUE || me.classe == PALADIN)
	 {
		if (me.wrongway)
		{
		 loga("getnear: Corrigindo wrongway (code 112).");
		 aperta(INTERACT);
		}
		else if (!me.melee)
		{
		 loga("getnear: Corrigindo distância (fora de melee) (code 221).");
		 aperta(INTERACT);
		}
		else
		{
		 //loga("getnear: Target em melee e facing correto — INTERACT de rotina. (code 223).");
		 aperta(INTERACT);
		}
	 }

	 // -----------------------
	 // CASTER CLASSES
	 // -----------------------
	 else // caster, warlock
	 {
		if (wlock.has_drain_soul)
		{
		 loga("getnear: Drain Soul ativo, não anda.");
		 return;
		}

		if (me.wrongway)
		{
		 loga("Wrongway ou out of range detectado. Corrigindo.");
		 aperta(INTERACT, me.outofrange ? 500 : 100); // gira para corrigir facing
		 loga("Interact code: 257"); // loga o código de interação
		 para(); // para de andar
		}
		else if (me.outofrange) // olhando pro lado errado?
		{
		 if (me.outofrange && !me.melee && me.autoattack)
		 {
			loga("Out of range do melee.");
			aperta(STOPCAST);
			return;
		 }
		}
	 }
	}



	// --------------------------------
	// MÉTODO STOPWAND: Para wand (sem retorno)
	// Se wand não está ativa, não faz nada.
	// Se wand ativa, força STOPCAST e espera até desativar ou timeout 3s.
	// Após parada, reativa autoattack.
	// --------------------------------
	void stopwand()
	{
	 if (!me.wandon)
		return; // não faz nada se wand não está ativa

	 int start_time = Environment.TickCount;
	 int elapsed=0;

	 clog("Parando wand...");

	 // Loop até wand sair ou timeout
	 while (me.wandon || flags.wandGCD > 0)
	 {
		checkme(); // atualiza me.wandon
		aperta(STOPCAST, 1);

		elapsed = Environment.TickCount - start_time;

		if (elapsed > 3000)
		{
		 clog("Timeout 3s ao esperar parar cast da wand.");
		 break;
		}
	 }

	 // Reativa autoattack
	 aperta(AUTOATTACK);

	 clog($"Wand desativada após {elapsed} ms.");
	}



	// --------------------------------
	// MÉTODO DE ESPERA VIGILANTE
	// --------------------------------
	void espera(int seconds=1)
	{
	 for (int i = seconds; i > 0; i--)
	 {
		if (me.combat)
		 return;
		wait(1000); // espera 1 segundo
		nao_afoga(); // verifica se não está afogando
		checkme(); // atualiza status do jogador
	 }
	}
	// ----------------------------------------
	// FUNÇÃO DEBUGCURA – Atualiza checkboxes da cura e chama logacura()
	// ----------------------------------------
	void debugcura(string ultima = "", string motivo = "", string idle = "")
	{
	 debug_gcd.Checked = (me.castbar > 0 && me.castbar <= 20 );
	 debug_forbearance.Checked = pala.forbearance;
	 debug_BOP.Checked = pala.BOP_up;
	 debug_dprot.Checked = pala.divine_protection_up;
	 debug_LOH.Checked = pala.cancast_LOH;
	 debug_potion.Checked = me.hp_potion_rdy;
	 debug_HOJ.Checked = pala.hoj_ready;

	 logacura(ultima, motivo, idle);  // atualiza textboxes e loga se checkbox marcada
	}


	// ----------------------------------------
	// FUNÇÃO LOGACURA – Preenche logs visuais da cura
	// Nunca apaga campos já preenchidos
	// Só substitui se vier conteúdo novo
	// ----------------------------------------
	void logacura(string ultima = "", string motivo = "", string idle = "")
	{
	 // preserva valores anteriores se os novos forem vazios
	 if (!string.IsNullOrEmpty(ultima)) tb_ultima_cura.Text = ultima;
	 if (!string.IsNullOrEmpty(motivo)) tb_motivo_cura.Text = motivo;

	 // idle_reason sempre sobrescreve
	 tb_idle_reason.Text = idle;

	 if (cb_logar_cura.Checked)
		loga($"[cura] ultima={ultima} motivo={motivo} idle={idle}");
	}

	// --------------------------------
	// MÉTODO DE CURA GERAL DO PALADINO
	// --------------------------------
	void tenta_curar()
	{
	 debugcura(); // atualiza os checkboxes de debug
	 int limiar_loh = 30; // hp abaixo desse valor pode disparar LOH
	 limiar_loh = atoi(tb_combatheal); // atoi(tb_LOH_limiar); // pega valor do textbox de limiar de LOH

	 int limiar_hp = atoi(tb_combatheal); // pega valor de hp considerado critico
	 if (pala.sol) limiar_hp -= 7; // se tem Seal of Light, permite segurar mais

	 string dbg = "";

	 dbg += "Forbearance..........: " + (pala.forbearance ? "tem" : "não tem") + "\r\n";
	 dbg += "Lay on Hands pronto..: " + (pala.cancast_LOH ? "sim" : "não") + "\r\n";
	 dbg += "BoP pronto...........: " + (pala.BOP_up ? "sim" : "não") + "\r\n";
	 dbg += "BoP ativo em você....: " + (pala.bop ? "sim" : "não") + "\r\n";
	 dbg += "Potion pronta........: " + (me.hp_potion_rdy ? "sim" : "não") + "\r\n";
	 dbg += "Decay estimado.......: " + tbdecay.Text + " hp/m" + "\r\n";
	 dbg += "Mobs no combate......: " + me.mobs.ToString() + "\r\n";

	 if (me.hp < limiar_hp) loga(dbg); // loga debug se hp estiver abaixo do limiar crítico


	 // -------------------------------------------------------------
	 // **NOVA CAMADA: CURAS ESPECIAIS PRIORITÁRIAS PARA 3 OU MAIS MOBS**
	 // Estas verificações são as primeiras a serem feitas.
	 // Se uma ação for tomada aqui e exigir, a função pode retornar imediatamente.
	 // -------------------------------------------------------------
	 if (me.mobs >= 3)
	 {
		// Regra 1: Vida abaixo de 50% => Usa poção em qualquer cenário (se disponível)
		if (me.hp < 50 && me.hp_potion_rdy)
		{
		 aperta(HEALTHPOTION);
		 loga("ALERTA (3+ mobs)! HP abaixo de 50%, usando Poção de Vida prioritariamente.");
		 wait(GCD); // Espera o Global Cooldown da poção
		 checkme();  // Atualiza o status após usar a poção
		 debugcura("POTION", "ativação prioritária em 3+ mobs - HP < 50%");
		 // Não damos 'return' aqui. Permitimos que o LoH (abaixo) seja avaliado
		 // caso a poção não seja suficiente ou as condições se mantenham.
		}

		// Regra 2: Vida abaixo de 40%, sem poção E sem proteções (incluindo Forbearance) => Usa Lay on Hands
		if (me.hp < 40 &&
				!me.hp_potion_rdy &&         // Sem poção
				!pala.BOP_up &&              // Sem Blessing of Protection
				!pala.divine_protection_up && // Sem Divine Protection
				!pala.divine_shield_up &&    // Sem Divine Shield
				pala.forbearance == false &&   // Forbearance deve ser false para usar LOH
				pala.cancast_LOH &&          // LOH deve estar pronto
				cb_loh.Checked)              // LOH deve estar permitido pelo checkbox
		{
		 aperta(LOH);
		 loga("EMERGÊNCIA (3+ mobs)! HP abaixo de 40% e sem proteções, usando Lay on Hands prioritariamente.");
		 debugcura("LOH", "ativação prioritária em 3+ mobs - HP < 40%, sem proteções");
		 return; // Lay on Hands é uma cura massiva, geralmente resolve a situação. Retorna imediatamente.
		}
	 }





	 // ----------------------------------------
	 // CURA COMUM – HLIGHT COM PROTEÇÃO
	 // ----------------------------------------
	 if (me.hp < limiar_hp && me.mana > 20 && !pala.forbearance)
	 {
		bool usou_protecao = false;
		string motivo = ""; // para registrar no log final

		if (pala.divine_shield_up && me.mana > 25)
		{
		 casta(DSHIELD); // usa Divine Shield
		 loga("Curando com divine shield.");
		 usou_protecao = true;
		 motivo = "cura com proteção (Divine Shield)";
		}
		else if (pala.divine_protection_up && me.mana > 25)
		{
		 casta(DPROT); // usa Divine Protection
		 loga("Curando com divine protection.");
		 usou_protecao = true;
		 motivo = "cura com proteção (Divine Protection)";
		}
		else if (pala.BOP_up && cb_BOP.Checked && me.mana > 25)
		{
		 casta(BOP); // usa Blessing of Protection
		 loga("Curando com blessing of protection.");
		 usou_protecao = true;
		 motivo = "cura com proteção (BOP)";
		}

		if (usou_protecao)
		{
		 castslow(HLIGHT);            // casta HLIGHT
		 
		 checkme();                // atualiza status após cast

		 debugcura("HLIGHT", motivo); // registra nos debug e caixas

		 if (me.level >= 20 && me.hp < 90)
		 {
			casta(FLASHHEAL);     // usa FLASHHEAL se necessário
			debugcura("FLASHHEAL", "HLIGHT nao curou suficiente, level >= 20, HP < 90"); // registra no debug
		 }
		 else if (me.mobs <2 && me.level < 20 && me.hp < 60)
		 {
			castslow(HLIGHT);            // casta mais um HLIGHT
			debugcura("OUTRO HLIGHT", "O primeiro nao curou suficiente, level < 20"); // registra no debug
			checkme();                // atualiza status após cast
		 }

		 if (me.hp >= limiar_hp)
			return;               // encerra se cura foi suficiente
		}
		else
		{
		 // nenhuma proteção disponível → não curou
		 debugcura("", "", "não curou – sem proteção disponível");
		 return;
		}
	 }


	 // ----------------------------------------
	 // NAO USOU PROTEÇÃO OU A CURA FOI POUCA.
	 // ----------------------------------------
	 // CURA CRÍTICA – POTION OU HLIGHT COM STUN
	 // ----------------------------------------
	 if (me.hp < limiar_hp)
	 {
		if (me.hp_potion_rdy)
		{
		 aperta(HEALTHPOTION);         // usa poção
		 loga("Curando com potion.");
		 wait(GCD);
		 checkme();

		 debugcura("POTION", "sem proteção, usou poção como recurso crítico");

		 if (me.hp >= limiar_hp)
			return;                   // cura resolveu
		}
		else if (pala.hoj_ready && pala.hoj_range)
		{
		 aperta(HOJ, GCD);            // stuna o alvo
		 loga("Curando com hammer of justice.");
		 castslow(HLIGHT);        // casta HLIGHT durante o stun
		 checkme();

		 debugcura("HLIGHT", "usou HOJ para interromper dano e conseguir curar");

		 if (me.hp >= limiar_hp)
			return;                   // cura resolveu
		}
		else
		{
		 // nenhum recurso crítico disponível
		 debugcura("", "", "não curou – sem poção e sem stun disponível");
		 return;
		}
	 }


	 // ----------------------------------------
	 // CURA DE EMERGÊNCIA – LOH OU CAST SECO
	 // ----------------------------------------
	 bool pode_usar_loh =
		 me.hp < limiar_loh &&
		 !me.hp_potion_rdy &&                  // não tem potion
		 !pala.BOP_up &&                       // não tem BOP
		 !pala.divine_protection_up &&        // não tem bubble
		 (!pala.hoj_ready || me.hp < 25) &&    // não tem stun ou tá no bico do corvo
		 pala.cancast_LOH &&                  // LOH disponível
		 cb_loh.Checked;

	 if (pode_usar_loh)
	 {
		aperta(LOH); // cura total com LOH
		loga("Emergência. Curando com lay on hands.");
		debugcura("LOH", "cura emergencial – sem nenhum outro recurso defensivo");
		return;
	 }


	 // ---------------------------------------------
	 // ÚLTIMA TENTATIVA: CURA SEM PROTEÇÃO
	 // ---------------------------------------------
	 if (me.hp < limiar_hp && me.mana > 20) // vida baixa e mana viável
	 {
		bool trocou_aura = false;       // flag pra saber se trocou aura
		byte aura_antiga = 0;           // salva a aura anterior pra restaurar depois

		// verifica se deve usar concentration aura temporariamente
		if (cb_concentration_aura.Checked && !pala.concentration)
		{
		 // salva e loga aura atual
		 if (pala.devotion)
		 {
			aura_antiga = DEVAURA;
			loga("Aura ativa: Devotion -> trocando para: Concentration");
		 }
		 else if (pala.retribution)
		 {
			aura_antiga = RETAURA;
			loga("Aura ativa: Retribution -> trocando para: Concentration");
		 }
		 else
		 {
			loga("Sem aura rastreável ativa -> trocando para: Concentration");
		 }

		 aperta(CONAURA, GCD);  // troca pra concentration aura
		 trocou_aura = true;    // marca que houve troca
		}

		loga("For the light!");         // tentativa heroica
		castslow(HLIGHT); // casta HLIGHT sem proteção
		checkme();                      // atualiza status

		// registra que castou HLIGHT no desespero
		debugcura("HLIGHT", "último recurso – castou no seco, com ou sem aura");

		// restaura aura antiga se tiver trocado
		if (trocou_aura && aura_antiga != 0)
		{
		 string nome_antiga = (aura_antiga == DEVAURA) ? "Devotion" : "Retribution";
		 loga("Cura finalizada -> restaurando aura: " + nome_antiga);
		 aperta(aura_antiga, GCD);
		}
	 }




	}

	// --------------------------------
	// MÉTODO KILLSTATS - GERENCIA ESTATÍSTICAS DE KILLS POR CLASSE
	// --------------------------------
	void killstats(string mode)
	{
	 // Array com todas as classes do WoW Classic
	 string[] wowClasses = { "priest", "paladin", "warlock", "warrior", "rogue", "shaman", "mage", "hunter", "druid" };

	 string settingsPath = Path.Combine(Application.StartupPath, "settings");
	 string filePath = Path.Combine(settingsPath, "montanha_totalkills.ini");

	 try
	 {
		if (mode == "load")
		{
		 // Cria pasta se não existir
		 if (!Directory.Exists(settingsPath))
			Directory.CreateDirectory(settingsPath);

		 // Se arquivo não existe, cria com valores zerados
		 if (!File.Exists(filePath))
		 {
			var defaultContent = new List<string> { "[totalkills]" };
			foreach (string className in wowClasses)
			{
			 defaultContent.Add($"{className}=0");
			}
			File.WriteAllLines(filePath, defaultContent);
			loga("Arquivo de kills criado com valores padrão.");
		 }

		 // Carrega valores do arquivo para as textboxes
		 var lines = File.ReadAllLines(filePath);
		 bool inTotalKillsSection = false;

		 foreach (string line in lines)
		 {
			string trimmedLine = line.Trim();

			if (trimmedLine == "[totalkills]")
			{
			 inTotalKillsSection = true;
			 continue;
			}

			if (trimmedLine.StartsWith("[") && trimmedLine != "[totalkills]")
			{
			 inTotalKillsSection = false;
			 continue;
			}

			if (inTotalKillsSection && trimmedLine.Contains("="))
			{
			 string[] parts = trimmedLine.Split('=');
			 if (parts.Length == 2)
			 {
				string className = parts[0].Trim();
				string value = parts[1].Trim();

				// Monta nome da textbox dinamicamente
				string textBoxName = $"tb_kills_{className}";
				TextBox targetTextBox = this.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

				if (targetTextBox != null)
				{
				 targetTextBox.Text = value;
				}
			 }
			}
		 }

		 loga("Estatísticas de kills carregadas do arquivo.");
		}
		else if (mode == "save")
		{
		 // Incrementa a textbox da classe atual com mobs mortos neste combate
		 int mobsKilled = killed_noskin.Count + killed_skin.Count;

		 if (mobsKilled > 0)
		 {
			// Descobre qual classe está sendo jogada
			string currentClassName = "";
			switch (me.classe)
			{
			 case PRIEST: currentClassName = "priest"; break;
			 case PALADIN: currentClassName = "paladin"; break;
			 case WARLOCK: currentClassName = "warlock"; break;
			 case WARRIOR: currentClassName = "warrior"; break;
			 case ROGUE: currentClassName = "rogue"; break;
			 case SHAMAN: currentClassName = "shaman"; break;
			 case MAGE: currentClassName = "mage"; break;
			 case HUNTER: currentClassName = "hunter"; break;
			 case DRUID: currentClassName = "druid"; break;
			 default:
				loga("Classe desconhecida. Ignorando estatísticas de kill.");
				return;
			}

			// Procura a textbox da classe atual
			string textBoxName = $"tb_kills_{currentClassName}";
			TextBox currentClassTextBox = this.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;

			if (currentClassTextBox == null)
			{
			 loga("Classe não implementada. Ignorando estatísticas de kill.");
			 return;
			}

			// Incrementa valor na textbox
			int currentKills = 0;
			int.TryParse(currentClassTextBox.Text, out currentKills);
			currentKills += mobsKilled;
			currentClassTextBox.Text = currentKills.ToString();

			// Salva arquivo atualizado
			var content = new List<string> { "[totalkills]" };

			foreach (string className in wowClasses)
			{
			 string tbName = $"tb_kills_{className}";
			 TextBox tb = this.Controls.Find(tbName, true).FirstOrDefault() as TextBox;

			 if (tb != null)
			 {
				content.Add($"{className}={tb.Text}");
			 }
			 else
			 {
				content.Add($"{className}=0");
			 }
			}

			// Cria pasta se não existir
			if (!Directory.Exists(settingsPath))
			 Directory.CreateDirectory(settingsPath);

			File.WriteAllLines(filePath, content);

			loga($"Kills atualizadas: +{mobsKilled} {currentClassName} (total: {currentKills})");
		 }
		}
	 }
	 catch (Exception ex)
	 {
		loga($"Erro ao {mode} estatísticas de kills: {ex.Message}");
	 }
	}


	int atoi(TextBox t) => int.Parse(t.Text); // retorna valor inteiro da textbox

	// --------------------------------
	// VERIFICA SE DEVE USAR JUDGEMENT
	// --------------------------------
	bool should_judge()
	{
	 if (me.level < 4) return false;                  // abaixo do nível 4, não tem skill
	 if (!cb_judge.Checked) return false;             // checkbox desativada → não julga
	 if (pala.judge_cd>0) return false;               // cooldown de Judgement ativo

	 bool ofensivo = pala.sor || pala.soc || pala.sotc; // seals ofensivos

	 if (cb_savemana.Checked && !mana(atoi(tb_mana_min)) && ofensivo)
		return false; // quer economizar mana com seal ofensivo

	 if ((tar.hp > 25 && !mana(15)) || (tar.hp <= 25 && !mana(4)))
		return false; // pouca mana → prioriza cura

	 if (!pala.sor && !pala.sow && !pala.sol && !pala.soc && !pala.sotc)
		return false; // nenhum seal ativo que permita julgamento

	 if (pala.sor)
	 {
		int min = int.Parse(tb_mana_min.Text);       // lê limiar mínimo de mana
		if (!mana(min) && tar.hp > 20)
		 return false; // pouca mana e mob ainda forte
	 }

	 // -----------------------------------------------
	 // EXCEÇÃO: SEAL OF LIGHT ATIVO → FORÇA JUDGEMENT
	 // julga sempre no pull, e no combate se valer a pena
	 // -----------------------------------------------
	 if (pala.sol)
	 {
		if (pala.jol) return false;                  // já tem debuff → não julga
		if (me.combat && tar.hp <= 25) return false; // em combate e mob já fraco → não julga
		return true;                                 // fora de combate ou vida alta → julga
	 }

	 // -----------------------------------------------
	 // EXCEÇÃO: SEAL OF THE CRUSADER ATIVO → FORÇA JUDGEMENT
	 // julga apenas se ainda não aplicou JOTC
	 // -----------------------------------------------
	 if (pala.sotc)
	 {
		if (pala.jotc) return false;                 // já tem debuff → não julga
		if (me.combat && tar.hp <= 25) return false; // mob fraco → não vale gastar
		return true;                                 // vida alta ou pull → julga
	 }

	 if (!pala.jud_range) return false;        // alvo fora de alcance
	 if (pala.judge_cd != 0) return false;     // cooldown ainda ativo

	 return true; // passou por todos os filtros → pode julgar
	}


	// --------------------------------
	// VERIFICA SE DEVE USAR HAMMER OF JUSTICE
	// --------------------------------
	bool should_stun()
	{
	 if (me.level < 8) return false;          // não tem skill ainda
	 if (!cb_use_hammer.Checked) return false; // checkbox desmarcada
	 if (!mana(13)) return false;              // mana insuficiente
	 if (!pala.hoj_ready) return false;        // cooldown ativo
	 if (!pala.hoj_range) return false;        // alvo fora do range
	 if (tar.casting) return true; // se o alvo estiver castando
	 if (me.hp < 40) return true;             // vida baixa → usa defensivamente

	 return false; // não atende critérios
	}
	//--------------------------------
	// ESPERA CAST EM ANDAMENTO
	//---------------------------
	void wait_cast()
	{
	 do
	 {
		checkme();
		wait(10);
	 } while (me.casting); // espera até não estar mais castando
	}

	// METODO WAITCAST - ESPERA CASTAR 																												
	// --------------------------------																												
	// MÉTODO MANA (TEM?)																													
	// --------------------------------
	public bool mana(int percent)
	{
	 return me.mana > percent;
	}

	// --------------------------------
	// MÉTODO HAS (BUFF)
	// --------------------------------
	public bool has(int buff)
	{
	 return buff > 0;
	}

	


private void bt_getstats_Click(object sender, EventArgs e)
{
getstats(ref me); // Chama o método getstats para atualizar o objeto player
	 loc alvo = new loc( // cria nova posição alvo
		 int.Parse(tb_debug1.Text), // X vem da textbox1
		 int.Parse(tb_debug2.Text)  // Y vem da textbox2
	 );

	}
	// --------------------------------------------------------------------
	// Função s(obj) - Converte .Text de qualquer controle em int (seguro)
	// --------------------------------------------------------------------
	int s(object obj)
	{
	 try
	 {
		var tipo = obj.GetType();                         // descobre o tipo do objeto
		var prop = tipo.GetProperty("Text");              // tenta achar a propriedade .Text
		if (prop == null)
		{
		 loga($"Objeto de tipo {tipo.Name} não tem .Text");
		 return -1;
		}

		string str = prop.GetValue(obj)?.ToString();      // extrai o texto
		return s(str);                                    // usa a função padrão
	 }
	 catch (Exception ex)
	 {
		loga($"Erro ao tentar converter objeto: {ex.Message}");
		return -1;
	 }
	}
	loc nloc(int x, int y) => new loc { x = x, y = y }; // mini metodo clock (cria loc de x e y)
	void movexy(int x, int y) => moveto(nloc(x, y)); // mini metodo move para xy 
	// ----------------------------------
	// BOTAO MOVETO (usa novo_moveto)
	// ----------------------------------
	private void bt_debug1_Click(object sender, EventArgs e)
	{
	 checkme(); 
	 
	 on = true;

	 loc go = nloc(int.Parse(tb_debug1.Text), int.Parse(tb_debug2.Text));

	 while (dist(me.pos, go) > 20) // enquanto a distância for maior que 1.5 metros
	 {
		 checkme(); // atualiza posição e facing
	 moveto(go);
		 }
	 on = false; // desativa o modo de movimento
	 para(); // para de andar
	}
	// ------------------------------------------
	// MÉTODO delta - Diferença angular entre me.facing e direção até b
	// Retorna valor entre -180 e +180 graus
	// ------------------------------------------
	int delta(loc a, loc b)
	{
	 int yaw = getyaw(a, b);                  // direção ideal entre a e b
	 int d = yaw - me.facing;                 // diferença bruta
	 if (d > 180) d -= 360;                   // ajusta para -180 a +180
	 if (d < -180) d += 360;                  // idem
	 return d;                                // delta final
	}



	// -----------------------------------------------------------
	// MÉTODO getyaw - Retorna facing ideal entre dois pontos
	// Usa correção do eixo Y se cb_humanlike estiver marcada
	// -----------------------------------------------------------
	int getyaw(loc orig, loc tar)


	{
	 double dx = tar.x - orig.x;               // diferença no eixo X
	 double dy = orig.y - tar.y;               // diferença no eixo Y (inverte por causa da orientação visual)

	 // ⭐ CORREÇÃO DO BUG: Tolerância para posição muito próxima, evita travar se estiver exatamente em cima do ponto
	 if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1)
	 {
		return me.facing; // mantém direção atual
	 }

	 if (true || cb_humanlike.Checked)                 // se modo humanlike estiver ativado
		dy /= 1.515;                          // divide pra compensar compressão do eixo Y

	 double ang = Math.Atan2(dx, dy) * (180.0 / Math.PI); // calcula ângulo visual
	 if (ang < 0) ang += 360;                 // ajusta ângulo negativo

	 double yaw = 360 - ang;                  // espelha para yaw no sentido horário
	 if (yaw >= 360) yaw -= 360;              // corrige wrap

	 return (int)Math.Round(yaw);             // retorna yaw arredondado
	}

	// ------------------------------------------
	// MÉTODO giraface(facing) - Gira até o ângulo desejado
	// ------------------------------------------
	void giraface(int f1, int d = 99999)
	{
	 checkme();                              // atualiza posição e facing atual
	 int f0 = me.facing;                     // facing atual
	 bool correndo = isdown(WKEY);           // está correndo?

	 int deltaR = (f0 - f1 + 360) % 360;     // ângulo pra direita
	 int deltaL = (f1 - f0 + 360) % 360;     // ângulo pra esquerda

	 bool direita = deltaR <= deltaL;        // decide direção
	 int delta = direita ? deltaR : deltaL;  // menor ângulo
	 bool logar = true;                      // flag para logar o giro em movimento

	 int tempo = Math.Min((int)(delta * 5.55), 1000); // tempo proporcional ao ângulo
	 float fator_drift = 1.25f;                        // fator de drift (ajuste fino pra não passar do ponto)
	 if (me.spd > 3) tempo = (int)(tempo * fator_drift); // aplica fator se estiver se movendo com velocidade

	 const byte BYTE_AKEY = 0x41;
	 const byte BYTE_DKEY = 0x44;
	 byte tecla = direita ? BYTE_DKEY : BYTE_AKEY;    // define tecla de giro (A ou D)

	 // ---------------------------
	 // PARADA PARA GIRO
	 // ---------------------------
	 if (cb_humanlike.Checked)
	 {
		if (false) // modo humano: para se a curva não for possível em movimento
		{
		 solta(WKEY);
		 logar = false;
		 aperta(PULA, 10);
		 loga("Curva impossível em movimento – parando para girar..");
		}
	 }
	 else // modo clássico: para se curva for brusca e o alvo estiver muito perto
	 {
		if (d >= 120 && ((d < 200 && tempo > 600) || tempo > 450))
		{
		 solta(WKEY);
		 logar = false;
		 aperta(PULA, 10);
		 loga("Curva abrupta – parando para girar. (no giralvo)");
		 loga($"Distância: {d}m - Tempo: {tempo}ms - Delta: {delta}°"); // loga detalhes da curva
		}
	 }

	 press(tecla);     // pressiona tecla de giro (A ou D)
	 wait(tempo);      // espera tempo proporcional ao ângulo
	 solta(tecla);     // solta a tecla

	 if (true)
	 {
		checkme(); // atualiza posição após o giro
		int f2 = me.facing; // facing após o giro
		int erro = ((f2 - f1 + 540) % 360) - 180; // erro com sinal
		if (!direita) erro = -erro; // corrige sinal se girou à esquerda
		int delta_total = delta; // delta original (pra uso futuro, se quiser logar)
														 // loga($"Erro final do giro: {erro}°"); // (opcional)
	 }

	 if (correndo) press(WKEY); // se estava andando antes, retoma
	}





	// --------------------------------
	// MÉTODO RODA
	// gira o bot para a esquerda ou direita baseado no ângulo desejado
	// --------------------------------
	void roda(int ang)
	{
	 if (ang == 0) return; // se ângulo for zero, não faz nada

	 if (ang < -180 || ang > 180) // checa se ângulo está fora do intervalo permitido  
	 {
		loga("erro: ângulo fora do intervalo -180 a +180");
		return;
	 }

	 int tempo = Math.Abs(ang) * 1000 / 180; // calcula o tempo proporcional (180° = 1000ms)  

	 if (ang > 0) // se ângulo for positivo, gira pra direita  
	 {
		loga("Girando " + ang + "° para direita");
		press(DKEY); // pressiona tecla de girar pra direita  
		wait(tempo); // espera o tempo proporcional  
		solta(DKEY); // solta a tecla  
	 }
	 else // se ângulo for negativo, gira pra esquerda  
	 {
		loga("Girando " + ang + "° para esquerda");
		press(AKEY); // pressiona tecla de girar pra esquerda  
		wait(tempo); // espera o tempo proporcional  
		solta(AKEY); // solta a tecla  
	 }
	}


	// ====================================================================
	// SISTEMA ANTI-STUCK UNIVERSAL
	// Para ser usado em qualquer loop que pode travar
	// ====================================================================
	// --------------------------------
	// VARIÁVEIS GLOBAIS (adicionar na sua classe)
	// --------------------------------
	private static loc lastpos = new loc(0, 0);
	private static int stuck_timer = 0;
	private static int stuck_count = 0;
	private static bool stuck_system_active = false;

	// --------------------------------
	// MÉTODO PRINCIPAL: check_stuck()
	// --------------------------------
	private int check_stuck()
	{
	 int now = Environment.TickCount;

	 // ================================
	 // AUTO-RESET: se mudou muito de posição
	 // ================================
	 if (stuck_timer > 0 && dist(me.pos, lastpos) > 50)
	 {
		loga("✅ Auto-reset: movimento significativo detectado");
		stuck_timer = 0;
		stuck_count = 0;
		stuck_system_active = false;
	 }

	 // ================================
	 // AUTO-RESET: se passou muito tempo sem verificar
	 // ================================
	 if (stuck_timer > 0 && (now - stuck_timer) > 60000)
	 {
		loga("🔄 Auto-reset: timeout de contexto (1min)");
		stuck_timer = 0;
		stuck_count = 0;
		stuck_system_active = false;
	 }

	 // ================================
	 // INICIALIZAÇÃO
	 // ================================
	 if (stuck_timer == 0)
	 {
		stuck_timer = now;
		lastpos = me.pos;
		stuck_count = 0;
		stuck_system_active = false;
		return 0;
	 }

	 // ================================
	 // VERIFICA A CADA 6 SEGUNDOS
	 // ================================
	 int elapsed = now - stuck_timer;
	 if (elapsed >= 6000)
	 {
		int distance_moved = dist(me.pos, lastpos);
		loga($"🚶 Movimento em 6s: {distance_moved} unidades");

		if (distance_moved < 10)
		{
		 stuck_count++;
		 loga($"🚫 TRAVAMENTO DETECTADO! Tentativa #{stuck_count}");

		 switch (stuck_count)
		 {
			case 1: // 6s: pulo simples
			 loga("🦘 Anti-stuck #1: Tentando pulo simples");
			 aperta(PULA, 100);
			 wait(500);
			 break;

			case 2: // 12s: rotina direita + clear target
			 loga("➡️ Anti-stuck #2: Rotina pela DIREITA + Clear Target");
			 execute_unstuck_routine(false);
			 aperta(CLEARTGT);
			 wait(1000);
			 return 2; // BREAK do loop

			case 3: // 18s: rotina esquerda
			 loga("⬅️ Anti-stuck #3: Rotina pela ESQUERDA");
			 execute_unstuck_routine(true);
			 wait(1000);
			 return 2; // BREAK do loop

			default: // 24s+: reset completo
			 loga("🔄 Anti-stuck #4+: RESET completo");
			 aperta(CLEARTGT);
			 para();
			 stuck_count = 0;
			 wait(2000);
			 return 3; // RETURN do método
		 }

		 return 1; // executou anti-stuck, pode continuar
		}
		else
		{
		 // Movimento normal detectado
		 if (stuck_count > 0)
		 {
			loga($"✅ Movimento normal retomado. Reset do sistema anti-stuck.");
			stuck_count = 0;
			stuck_system_active = false;
		 }
		}

		// Atualiza para próxima verificação
		stuck_timer = now;
		lastpos = me.pos;

		return 0;
	 }

	 return 0; // ainda não passou 6s
	}
	// --------------------------------
	// ROTINA ANTI-STUCK SIMPLIFICADA
	// --------------------------------
	private void execute_unstuck_routine(bool esquerda)
	{
	 loga($"🔧 Executando rotina anti-stuck: {(esquerda ? "ESQUERDA" : "DIREITA")}");

	 para();
	 wait(200);

	 // ETAPA 1: Anda para trás por 5 segundos
	 loga("⬇️ Andando para trás por 5 segundos");
	 anda(-4.0f);  // marcha à ré por 5 segundos
	 

	 // ETAPA 2: Roda 70 graus para direita ou esquerda
	 if (esquerda)
	 {
		loga("↺ Girando 70° para ESQUERDA");
		roda(-70);  // gira 70° para esquerda
	 }
	 else
	 {
		loga("↻ Girando 70° para DIREITA");
		roda(70);   // gira 70° para direita
	 }
	 

	 // ETAPA 3: Anda para frente por 5 segundos
	 loga("⬆️ Andando para frente por 5 segundos");
	 anda(4.0f);   // marcha à frente por 5 segundos

	 espera(1);
	 loga("✅ Rotina anti-stuck concluída");
	}

	// --------------------------------
	// RESET MANUAL (OPCIONAL)
	// --------------------------------
	private void reset_stuck_system()
	{
	 stuck_timer = 0;
	 stuck_count = 0;
	 stuck_system_active = false;
	 lastpos = new loc(0, 0);
	 loga("🔄 Sistema anti-stuck resetado manualmente");
	}



	// --------------------------------  
	// MÉTODO ANDA  
	// faz o bot andar pra frente por X segundos, ou apenas iniciar/parar marcha  
	// --------------------------------  
	void anda(float s)
	{
	 if (s == 0) // se for 0, apenas começa a andar  
	 {
		press(WKEY); // segura pra frente  
		loga("iniciou marcha contínua");
		return;
	 }
	 if (s == -1) // se for -1, para de andar  
	 {
		solta(WKEY); // solta a tecla  
		loga("parou de andar");
		return;
	 }
	 if (s < -1) // valores menores que -1: marcha à ré
	 {
		float segundos_re = Math.Abs(s); // converte para positivo
		int tempos = (int)(segundos_re * 1000); // converte segundos para milissegundos
		press(SKEY); // começa a andar para trás
		wait(tempos); // espera o tempo em segundos (convertido para ms)
		solta(SKEY); // para de andar para trás
		loga("andou para trás por " + segundos_re + " segundos");
		return;
	 }

	 int tempo = (int)(s * 1000); // converte segundos para milissegundos
	 press(WKEY); // começa a andar  
	 wait(tempo); // espera o tempo em segundos (convertido para ms)
	 solta(WKEY); // para de andar  
	 loga("andou por " + s + " segundos");
	}

	private void bt_debug2_Click(object sender, EventArgs e)
{
	 checkme();
tb_debug1.Text = me.pos.x.ToString();
tb_debug2.Text = me.pos.y.ToString();

}
	// LOCALIZADOR DA FLEXA DO MINIMAP
	private Point localiza_flexa_brilho(Bitmap bmp, Point estimado)
	{
	 int cx = estimado.X;
	 int cy = estimado.Y;
	 int margem = 5; // varredura de 5px pra cada lado (total 11x11)

	 int xmin = Math.Max(0, cx - margem);
	 int xmax = Math.Min(bmp.Width - 1, cx + margem);
	 int ymin = Math.Max(0, cy - margem);
	 int ymax = Math.Min(bmp.Height - 1, cy + margem);

	 int brilho_max = -1;
	 Point melhor = new Point(-1, -1);

	 for (int y = ymin; y <= ymax; y++)
	 {
		for (int x = xmin; x <= xmax; x++)
		{
		 Color c = bmp.GetPixel(x, y);
		 int brilho = c.R + c.G + c.B;

		 if (brilho > brilho_max)
		 {
			brilho_max = brilho;
			melhor = new Point(x, y);
		 }
		}
	 }

	 loga($"Flecha localizada por brilho: {melhor.X}, {melhor.Y} (Brilho={brilho_max})");
	 return melhor;
	}

	// --------------------------------------------
	// BOTÃO DEBUG3: CAPTURA MANUAL DO MINIMAPA
	// --------------------------------------------
	private void bt_debug3_Click(object sender, EventArgs e)
	{
	 get_minimap(); // chama método novo
	 loga("Captura manual feita."); // opcional
	}


	// -------------------------------------------------
	// MÉTODO LOAD_SETTINGS - Carrega CheckBox e TextBox
	// -------------------------------------------------
	private void load_settings()
	{
	 if (!File.Exists("discord.cfg")) return; // se não existe, sai do método

	 string[] linhas = File.ReadAllLines("discord.cfg"); // lê todas as linhas do arquivo

	 foreach (string linha in linhas) // percorre cada linha
	 {
		if (string.IsNullOrWhiteSpace(linha)) continue; // ignora linhas em branco
		if (!linha.Contains("=")) continue; // ignora linhas inválidas

		string[] partes = linha.Split('='); // separa nome e valor
		if (partes.Length != 2) continue; // ignora se não tem duas partes

		string nome = partes[0]; // nome do controle
		string valor = partes[1]; // valor a ser atribuído

		Control[] encontrados = this.Controls.Find(nome, true); // busca recursiva por nome

		if (encontrados.Length == 0) continue; // se não achou, ignora

		Control c = encontrados[0]; // pega o primeiro controle encontrado

		if (c is CheckBox cb)
		 cb.Checked = (valor == "true"); // define estado do checkbox

		else if (c is TextBox tb)
		 tb.Text = valor; // define texto do textbox
	 }
	 try
	 {
		int x = int.Parse(val("win_left", linhas));
		int y = int.Parse(val("win_top", linhas));
		int w = int.Parse(val("win_width", linhas));
		int h = int.Parse(val("win_height", linhas));

		string monitor = val("win_monitor", linhas);

		// verifica se o monitor ainda existe
		foreach (var tela in Screen.AllScreens)
		{
		 if (tela.DeviceName == monitor)
		 {
			this.StartPosition = FormStartPosition.Manual; // define posição manual
			this.Location = new Point(x, y);               // aplica posição
			this.Size = new Size(w, h);                    // aplica tamanho
			break;
		 }
		}
	 }
	 catch { } // ignora erros se algo estiver faltando

	}
	// CONTINUAÇAO DO ACIMA
	private string val(string chave, string[] linhas)
	{
	 foreach (string linha in linhas)
		if (linha.StartsWith(chave + "="))
		 return linha.Substring(chave.Length + 1);

	 return "0"; // valor padrão
	}

	// ----------------------------------------------------------
	// MÉTODO CASTSLOW: aperta e espera o cast
	//-----------------------------------------------------------
	void castslow(int key)
	{
	 aperta((byte)key, 1000); // aperta a tecla e espera o GCD
	 do
	 { 
	 checkme();
		wait (10); // espera 10ms
	 } while (me.casting); // enquanto estiver castando, continua esperando
	}

	// ----------------------------------------------------------
	// MÉTODO CASTA: aperta e espera o GCD 
	//-----------------------------------------------------------
	void casta(int key, bool stopcast = false)
	{
	 if (stopcast && me.wandon) // se for pra parar o cast
	 {
		aperta(STOPCAST);
		wait(1000);
	 }
		aperta((byte)key, 1450); // aperta a tecla e espera o GCD
	 checkme();
	}

	// ----------------------------------------------------------
	// MÉTODO RECURSIVO - Adiciona CheckBox e TextBox na lista (pra salvar o cfg)
	// ----------------------------------------------------------
	private void coleta_controles(Control pai, List<string> linhas)
	{
	 foreach (Control c in pai.Controls) // percorre todos os filhos
	 {
		if (c is CheckBox cb)
		 linhas.Add(cb.Name + "=" + cb.Checked.ToString().ToLower()); // salva checkbox

		else if (c is TextBox tb)
		 linhas.Add(tb.Name + "=" + tb.Text); // salva textbox

		if (c.HasChildren) // se esse controle tiver filhos também
		 coleta_controles(c, linhas); // chama recursivamente
	 }
	}

	//	┌───────────────┬───────────────────────────────┬───────────────┐
	//  │ Zona Inferior │      Zona Intermediária       │ Zona Superior │
	//  │ (força SOL)   │ Depende do modo anterior      │ (força SOR)   │
	//  └───────────────┴───────────────────────────────┴───────────────┘
	//hp<L            L ≤ hp ≤ H                  hp> H

	// ----------------------------------------
	// MÉTODO BEST_SEAL
	// Define o melhor Seal com base em combate, vida,
	// intenção de aplicar JOL ou JOTC, e modo defensivo
	// ----------------------------------------
	byte best_seal()
	{
	 // ----------------------------------------
	 // JOL – aplica SOL se mob não tiver debuff
	 // Prioridade máxima
	 // ----------------------------------------
	 if (tar.hp > 10) // mob precisa estar com vida razoável
	 {
		if (cb_keep_JOL.Checked && !pala.jol)
		{
		 if (pala.sol) return 255; // já está ativo → não recasta
		 return SOL; // aplica SOL pra julgar JOL
		}

		// ----------------------------------------
		// JOTC – aplica SOTC se mob não tiver debuff
		// Só se for mob relevante (nível suficiente)
		// ----------------------------------------
		if (cb_keep_JOTC.Checked && !pala.jotc)
		{
		 if (pala.sotc) return 255; // já está com SOTC → não recasta
		 if (pala.sor) return 255;  // nunca substitui SOR por SOTC
		 int dif = int.Parse(tb_JOTC_lvl.Text); // diferença mínima de nível
		 if (tar.level >= me.level + dif)
			return SOTC; // aplica SOTC pra julgar JOTC
		}
	 }

	 // ----------------------------------------
	 // BLOQUEIA SEAL SE NÃO TIVER MANA OU SPELLS
	 // ----------------------------------------
	 if (!mana(7)) return 255; // sem mana pra castar seal
	 if (!cb_SOL.Checked && !cb_SOR.Checked) return 255; // nenhum seal habilitado
	 if (pala.sol || pala.sow) return 255; // não remove seals emergenciais ativos

	 // ----------------------------------------
	 // DEFINE OS LIMITES DE HISTERSE
	 // ----------------------------------------
	 int L = int.Parse(tb_SOL_limiar.Text); // limiar inferior (ativa SOL)
	 int H = 60;                             // limiar superior (ativa SOR)

	 // ----------------------------------------
	 // DETERMINA A REGIÃO E AJUSTA O ESTADO
	 // ----------------------------------------
	 if (me.hp < L)
		pala.defseal = true;  // entra em modo defensivo
	 else if (me.hp > H)
		pala.defseal = false; // sai do modo defensivo

	 // ----------------------------------------
	 // DECIDE O SEAL COM BASE NO ESTADO ATUAL
	 // ----------------------------------------
	 byte seal = 255;

	 if (pala.defseal && cb_SOL.Checked)
		seal = SOL;           // modo defesa → usa SOL
	 else if (!pala.defseal && cb_SOR.Checked)
		seal = SOR;           // modo ofensivo → usa SOR

	 // ----------------------------------------
	 // JULGA ANTES DE TROCAR DE SEAL (SE POSSÍVEL)
	 // ----------------------------------------
	 if (seal != 255 && pala.jud_range && pala.judge_cd == 0)
	 {
		bool mudando =
			(seal == SOL && !pala.sol && !pala.jol) ||     // julga SOL se JOL ainda não ativo
			(seal == SOTC && !pala.sotc && !pala.jotc) ||  // julga SOTC se JOTC ainda não ativo
			(seal == SOR && !pala.sor);                    // julga SOR sempre antes de trocar

		if (mudando)
		 aperta(JUDGEMENT); // aproveita seal atual antes de trocar
	 }



	 if ((seal == SOR && pala.sor) ||
		(seal == SOL && pala.sol) ||
		(seal == SOTC && pala.sotc))
		return 255; // evita recast do mesmo seal

	 return seal; // retorna seal escolhido
	}



	// FIM DOS METODOS GLOBAIS 
	//-------------------------------------- 
	// MÓDULO DE NAVEGAÇÃO - TAB NAVEGAÇÃO
	//--------------------------------------
	// MÉTODOS
	// Checkme() faz chamada para getstats, para retrocompatibilidade com amigão
	void checkme()
{
	 wait(50);
getstats(ref me);
	 
}
	void clog(string str)
	{
	 lb_combatlog.AppendText(str + "\r\n"); // adiciona o texto e pula pra próxima linha
	}
	// MÉTODO LOGA - ESCREVE NO LOG E DÁ ENTER AUTOMATICAMENTE
	void loga(string str)
{
lb_log.AppendText(str + "\r\n"); // adiciona o texto e pula pra próxima linha
}
// PACK: converte loc para string "xx,yy,zz,ww" para popular a listbox
static string pack(loc coord)
{
int intx = coord.x / 100; // parte inteira do x
int decx = coord.x % 100; // parte decimal do x
int inty = coord.y / 100; // parte inteira do y
int decy = coord.y % 100; // parte decimal do y

return $"{intx},{decx},{inty},{decy}";
}

// UNPACK: converte string "xx,yy,zz,ww" para loc PARA POPULAR A LISTA DE WAYPOINTS
static loc unpack(string coord)
{
string[] parts = coord.Split(',');

int intx = int.Parse(parts[0]); // parte inteira do x
int decx = int.Parse(parts[1]); // parte decimal do x
int inty = int.Parse(parts[2]); // parte inteira do y
int decy = int.Parse(parts[3]); // parte decimal do y

int x = (intx * 100) + decx;
int y = (inty * 100) + decy;

return new loc { x = x, y = y };
}

	// função que move o cursor.... (em construção) 
	public static void mousemove(int x, int y)
	{
	 Cursor Cursor = new Cursor(Cursor.Current.Handle);
	 Cursor.Position = new Point(x, y);
	}
	public static void DoMouseClick(int botao = 1)
	{
	 //Call the imported function with the cursor's current position
	 int X = Cursor.Position.X;
	 int Y = Cursor.Position.Y;
	 if (botao == 1) mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
	 else if (botao == 2)
		mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
	}

	public static CURSORINFO oldmouse;
	//----------------------------------
	// VE SE MUDOU CURSOR NAS NOVAS COORDENADAS
	//---------------------------------
	bool mudoucursor(int x, int y)
	{
	 DoMouseClick();
	 wait(200);
	 oldmouse.hCursor = getcursor();
	 mousemove(x, y);
	 DoMouseClick();
	 wait(200);
	 if (oldmouse.hCursor == getcursor()) return false;
	 return true;
	}

	// Clica em ponto na tela 

	// --------------------------------
	// MÉTODO NEAR_HASH
	// Retorna a menor distância entre um loc e uma coleção (lista ou hashset)
	// Retorna int.MaxValue se a coleção estiver vazia
	// --------------------------------
	int near_hash(loc origem, IEnumerable<loc> colecao)
	{
	 int menor = int.MaxValue; // inicializa com valor alto

	 foreach (loc p in colecao) // percorre todos os pontos
	 {
		int d = dist(origem, p); // calcula distância entre origem e ponto
		if (d < menor)           // se encontrou distância menor
		 menor = d;           // atualiza distância mínima
	 }

	 return menor; // retorna a menor distância encontrada
	}


	// --------------------------------
	// MÉTODO NEAREST
	// Retorna o índice do loc mais próximo da lista
	// Retorna -1 se lista vazia
	// --------------------------------
	int nearest(loc origem, List<loc> lista)
{
if (lista.Count == 0) return -1; // lista vazia, retorna -1

int melhor = 0; // melhor índice encontrado
int menor = dist(origem, lista[0]); // distância do primeiro elemento

for (int i = 1; i < lista.Count; i++) // percorre a lista
{
 int d = dist(origem, lista[i]); // calcula a distância para o item atual
 if (d < menor) // se for menor que a menor distância
 {
	menor = d; // atualiza a menor distância
	melhor = i; // atualiza o melhor índice
 }
}

return melhor; // retorna o índice do loc mais próximo
}


// BOTAO ADD WAYPOINT 
private void button2_Click(object sender, EventArgs e)
{
checkme(); // Obtém posição atual (me.pos)
loc newLoc = me.pos;

// Verifica duplicata
if (lwp.Any(loc => loc.x == newLoc.x && loc.y == newLoc.y))
{
 loga("Waypoint não adicionado: Já existente.");
}
else
{
 lwp.Add(newLoc); // Adiciona waypoint à lista na memória
 string packed = pack(newLoc);
 lbwp.Items.Add(packed); // Empacota coordenada e adiciona à lista visual
 loga($"Waypoint {packed} adicionado.");
 lbwp.SelectedIndex = 0; // Seleciona primeira posição
}
}
// BOTAO GOTO WAYPOINT Moves to selected waypoint in the list.
private void button3_Click(object sender, EventArgs e)
{
if (lbwp.SelectedItem == null)
{
 loga("Nenhum waypoint selecionado.");
 return;
}

string selected = lbwp.SelectedItem.ToString();
loc target = unpack(selected);
loga($"Moving to: Target.x = {target.x}  Target.y = {target.y}");
 moveto(target);
	 solta(WKEY); // para quando chega
}

private void button1_Click(object sender, EventArgs e)
{
	 
	 wait(1000);
on = false;
	 wait(1000);
	 // ENCERRA LEITURA EM BACKGROUND DOS STATS 
	 StopBackgroundUpdates(); // <- ADICIONAR AQUI
	}

private void button4_Click(object sender, EventArgs e)
{
Clipboard.SetText(lb_log.Text); // copia todo o texto para a área de transferência
loga("Log copiado para área de transferência.\r\n"); // confirma no próprio log
	 lb_log.Text = ""; // limpa
}

	// --------------------------------  
	// MÉTODO UNSTUCK  
	// --------------------------------  
	void unstuck()
	{
	 loc old = me.pos;
	 loga("unstuck: pulo");
	 aperta(WKEY, 0);         // garante que W esteja pressionado
	 aperta(SPACEBAR, 100);   // tenta pular
	 wait(1000);
	 checkme();
	 int andou = dist(me.pos, old);
	 if (andou>2)         // se começou a andar, pulo funcionou
	 {
		//loga($"andou {andou}m, pulo resolveu, movimento retomado");
		stuckcount = 0;
		return;              // sai do método, nada mais a fazer
	 }

	 // se chegou aqui, é porque o pulo não resolveu
	 //loga("pulo falhou, girando e andando");
	 //roda(80);                // gira 80° pra direita
	 //anda(6);                 // anda 6 metros
	 //aperta(WKEY, 0);         // garante que siga andando
	 loga("executando manobra: giro reverso e avanço");
	 roda(-170);   // gira 170° pra esquerda (volta quase para tras)
	 anda(7);      // anda 6 metros em nova direção
	 roda(80); // ou seja resultado final é virar para a esquerda 
	 anda(7); // anda 6 metros para a esquerda 
	 if (andou > 7)         // se começou a andar, pulo funcionou
	 {
		loga($"andou {andou}m, andada resolveu, movimento retomado");
		stuckcount = 0;
		return;
	 }
	}
	// -------------------------------------------------------
	// BOTÃO: INICIAR PERCURSO DE WAYPOINTS (Linear ou Circular)
	// Percorre os pontos de lwp a partir do ponto mais próximo.
	// Usa uma variável de direção (dir = +1 ou -1) para alternar
	// entre ida e volta no modo linear. Evita reversões de lista.
	// Se nostop estiver marcado, o percurso é contínuo.
	// Loga a inversão com mensagem no debug.
	// -------------------------------------------------------
	private void button1_Click_1(object sender, EventArgs e)
	{
	 bt_save_cfg_Click(sender, e);
	 {

		// ATUALIZADOR EM BACKGROUND DOS STATS 
		//StartBackgroundUpdates(); // <- ADICIONAR AQUI
															

		int hs_tick = 0; // guarda o último tick registrado (global ou da classe)

	 if (cb_HS_timer.Checked && hs_tick == 0) // só na primeira vez
	 {
		hs_tick = Environment.TickCount; // marca início do controle

		int horas = 0;
		int.TryParse(tb_timer_hours.Text, out horas); // lê quantas horas rodar

		DateTime fim = DateTime.Now.AddHours(horas); // calcula horário final
		hs_min_left.Text = fim.ToString("dd/MM/yy HH:mm"); // salva no textbox
	 }

	 last_kill_time = Environment.TickCount; // tempo do último combate vencido
																							 // --------------------------------
	 on = true;                        // ativa o bot

// CARREGA APENAS WAYPOINTS VÁLIDOS
// --------------------------------
		lwp.Clear();

		foreach (var item in lbwp.Items)
		{
		 string linha = item.ToString();

		 // Só adiciona se for waypoint (3 vírgulas = 4 números)
		 if (linha.Count(c => c == ',') == 3)
		 {
			lwp.Add(unpack(linha));
		 }
		}

		if (lwp.Count == 0)
		{
		 loga("❌ Nenhum waypoint válido encontrado!");
		 return;
		}

		checkme();                        // atualiza posição do drone

	 int idx = nearest(me.pos, lwp);   // encontra o ponto mais próximo
	 if (idx == -1) return;

	 indexAtual = idx; // começa a partir do ponto mais próximo
	 int dir = +1;      // direção inicial: para frente

	 while (on)
	 {
		//-----------------------------
		// CONTROLE DE TIMER PARA HS 
		//-----------------------------
		if (cb_HS_timer.Checked)
		 if (Environment.TickCount - hs_tick >= 60000) // passou 1 minuto
		 {
			hs_tick = Environment.TickCount;

			DateTime fim;
			if (DateTime.TryParse(hs_min_left.Text, out fim))
			 if (DateTime.Now >= fim)
				HS(); // executa a finalização
		 }

		//-----------------------------
		lbwp.SelectedIndex = indexAtual;         // destaca na interface

		if (dist(me.pos, lwp[indexAtual]) < 1) continue; // já está no ponto → ignora

		moveto(lwp[indexAtual]); // move até o ponto atual

		indexAtual += dir; // avança na direção atual

		// --------- CONTROLE DE EXTREMOS ---------
		if (indexAtual >= lwp.Count) // passou do fim
		{
		 if (cb_round.Checked )
		 {
			indexAtual = 0;               // modo circular → volta ao início
			loga("Loop completo. Recomeçando do início.");
		 }
		 else if (true)
		 {
			dir = -1;                     // modo linear → inverte direção
			indexAtual = lwp.Count - 2;   // volta pro penúltimo
			loga("Término do caminho. Voltando.");
		 }
		 else break;                       // modo sem nostop → encerra
		}
		else if (indexAtual < 0) // passou do início
		{
		 if (cb_round.Checked )
		 {
			indexAtual = lwp.Count - 1;   // modo circular → vai pro final
			loga("Loop completo. Recomeçando do final.");
		 }
		 else if (true)
		 {
			dir = +1;                     // modo linear → inverte direção
			indexAtual = 1;               // vai pro segundo
			loga("Término do caminho. Indo novamente.");
		 }
		 else break;                       // modo sem nostop → encerra
		}
	 }
	}
	}


	// --------------------------------
	// MÉTODO CARREGAR_WAYPOINTS (MODIFICADO)
	// Carrega waypoints ignorando cercas
	// --------------------------------
	private void carregar_waypoints()
	{
	 string nome = "waypoints.txt";

	 if (File.Exists("discord.ini"))
	 {
		string[] linhascfg = File.ReadAllLines("discord.ini");
		if (linhascfg.Length > 0 && linhascfg[0].Trim() != "")
		 nome = linhascfg[0].Trim();
	 }

	 tb_filename.Text = nome;

	 if (File.Exists(nome))
	 {
		string[] linhas = File.ReadAllLines(nome);
		lbwp.Items.Clear();
		lwp.Clear();         // Limpa lista de waypoints para navegação
		lcercas.Clear();     // Limpa lista de cercas

		int waypoints_carregados = 0;
		int cercas_carregadas = 0;

		foreach (string linha in linhas)
		{
		 int virgulas = linha.Count(c => c == ',');

		 if (virgulas == 3) // waypoint (4 números)
		 {
			lbwp.Items.Add(linha);
			lwp.Add(unpack(linha));  // Adiciona à lista de navegação
			waypoints_carregados++;
		 }
		 else if (virgulas == 11) // cerca (12 números)
		 {
			try
			{
			 Cerca cerca = Cerca.FromPacked(linha);
			 lcercas.Add(cerca);
			 lbwp.Items.Add(linha);
			 cercas_carregadas++;

			 // Log detalhado da cerca
			 loga($"Cerca carregada: A({cerca.pontoA.x},{cerca.pontoA.y}) " +
						$"B({cerca.pontoB.x},{cerca.pontoB.y}) " +
						$"P({cerca.pontoP.x},{cerca.pontoP.y})");
			}
			catch (Exception ex)
			{
			 loga($"Erro ao carregar cerca: {ex.Message}");
			}
		 }
		 else if (!string.IsNullOrWhiteSpace(linha))
		 {
			loga($"⚠️ Linha com formato inválido ignorada: {linha}");
		 }
		}

		if (true)
		{
		 loga($"Arquivo {nome} carregado");
		 loga($"Total: {waypoints_carregados} waypoints navegáveis");
		 loga($"        {cercas_carregadas} cercas carregadas");
		}
	 }
	 else
	 {
		loga($"Arquivo {nome} não encontrado");
	 }
	}


	// --------------------------------
	// BOTÃO START RECORDING
	// Começa a gravar os pontos percorridos pelo player
	// --------------------------------
	private void button5_Click(object sender, EventArgs e)
{
{
 // Limpa listas de waypoints
 lbwp.Items.Clear();
 lwp.Clear();

 // Atualiza posição inicial
 checkme();
 loc lastpos = me.pos;

 // Já adiciona o ponto inicial
 string firstpacked = pack(me.pos);
 lbwp.Items.Add(firstpacked);

 // Lê o valor mínimo de distância para gravar (ex.: 250)
 int lim;
 if (!int.TryParse(tb_WP_distance.Text, out lim)) lim = 250; // padrão 250 se der erro

 on = true; // garante que o loop vai rodar

 while (on) // enquanto bot estiver ativo
 {
	wait(500); // espera 1 segundo
	checkme(); // atualiza posição atual

	int d = dist(me.pos, lastpos); // calcula distância desde o último ponto


	if (d >= lim) // se a distância for maior que o limite
	{
	 lastpos = me.pos; // atualiza último ponto
	 string packed = pack(me.pos); // empacota posição atual
	 lbwp.Items.Add(packed); // adiciona na listbox
	}
 }
}
}
// LIMPA LISTBOX
private void button6_Click(object sender, EventArgs e)
{
	 // Limpa lista visual
	 lbwp.Items.Clear();

	 // Limpa lista de waypoints para navegação
	 lwp.Clear();

	 // Limpa lista de cercas
	 lcercas.Clear();

	 // Log para confirmação
	 loga("🧹 Waypoints e cercas COMPLETAMENTE LIMPOS!");
	}

	// ------------------------------------------
	// VARIAVEIS GLOBAIS DE ESTATISTICA DE LOOT (usa no metodo abaixo) 
	// ------------------------------------------
	int[] lootfreq = new int[25]; // contador de frequência por posição
	int total_loots = 0;          // total de localizações válidas

	// ---------------------------------------------
	// METODO LOGASTATS: MOSTRA FREQUENCIA DE LOOT ADAPTATIVO
	// ---------------------------------------------
	void logastats()
	{
	 int total = fila_fifo.Count; // total adaptativo dos últimos loots
	 if (total == 0) return;      // evita divisão por zero

	 string linha = ""; // acumula string do log

	 for (int i = 0; i < 25; i++)
	 {
		if (lootfreq[i] == 0) continue;                 // ignora posições zeradas
		int pct = lootfreq[i] * 100 / total;            // percentual da janela
	//	linha += $"[{i:00}]: {pct}% ";                  // adiciona ao texto
	 }

	 //if (linha != "") loga("Estatísticas recentes: " + linha.Trim()); // loga final
	}


	// VARIAVEIS GLOBAIS DE LOOT
	
	loc last_success = new loc { x = -1, y = -1 }; // último ponto que deu loot
	double peso_otimo = 10.0; // valor padrão, será carregado do arquivo
	double exp_otimo = 1.0;   // idem
	DateTime prox_salva = DateTime.Now.AddMinutes(10); // inicial
	byte ultimo_tipo_loot = 0; // marca o ultimo tipo de loot, usado para skin no mesmo local 
	bool primeira_tentativa_sessao = true; // marca se é a primeira tentativa de loot na sessão

	// ----------------------------------------
	// FUNÇÃO tipocur
	// Retorna o tipo do cursor atual (como byte)
	// Renderiza e calcula checksum da imagem do cursor
	// ----------------------------------------
	public byte tipocur()
	{
	 IntPtr cur = getcursor();       // obtém o cursor atual
	 IntPtr bmp = rendercursor(cur); // renderiza imagem do cursor
	 byte[] img = getpixels(bmp);    // extrai os pixels da imagem
	 return checksum(img);           // retorna o byte identificador do cursor
	}




	// ---------------------------------------------
	// MÉTODO SCANLOOT: VERSÃO COM DESCARTE PARCIAL
	// ---------------------------------------------
	// VARIAVEIS DE USO PARA O METODO IDENTIFICAR OS CURSORES
	// handle mais comum (saquinho)
	public IntPtr lootCursor = IntPtr.Zero;
		// handle secundário (skinning)
	public IntPtr skinCursor = IntPtr.Zero;
		// trava o aprendizado depois de 5 combates
	public bool cursors_travados = false;
		// contador de combates com loot analisado
	public int lootCombates = 0;
	// uso de Dictionary<IntPtr, int> para contar frequência (rápido e eficiente)
	Dictionary<IntPtr, int> cursor_freq = new Dictionary<IntPtr, int>();
	// uso de Dictionary<IntPtr, loc> para lembrar a última posição observada de cada cursor
	Dictionary<IntPtr, loc> cursor_pos = new Dictionary<IntPtr, loc>();






	// ---------------------------------------------
	// MÉTODO scanloot
	// Varre 25 pontos ao redor do personagem para detectar loot
	// Começa pelo centro (fixo), depois tenta 24 posições circulares
	// Posições são ordenadas por frequência, com chance de pular as de freq 0
	// Frequência da posição 0 (centro) nunca é salva nem usada na ordenação
	// ---------------------------------------------
	// --------------------------------
	// MÉTODO SCANLOOT OTIMIZADO
	// Versão com início inteligente na posição do último loot
	// --------------------------------
	public loc scanloot()
	{
	 int pausa = 30; // delay padrão entre movimentos do mouse
	 const double CHANCE_PULAR_ZERO_FREQ = 0.65; // chance de ignorar ponto com freq 0

	 if (DateTime.Now >= prox_salva)
	 {
		salva_loot(); // salva vetor e aplica trim se necessário
		prox_salva = DateTime.Now.AddMinutes(5);
	 }

	 focawow(); // traz o WoW pra frente

	 int w = Screen.PrimaryScreen.Bounds.Width;
	 int h = Screen.PrimaryScreen.Bounds.Height;

	 int cx = w / 2;
	 int cy = (int)(h / 2 + h * 0.05); // centro levemente abaixo da linha horizontal

	 // --------------------------------
	 // INÍCIO INTELIGENTE: TENTA LAST_SUCCESS PRIMEIRO
	 // --------------------------------
	 if (cb_skinning.Checked                    // skinning ativo
			 && !primeira_tentativa_sessao          // não é a primeira tentativa da sessão
			 && ultimo_tipo_loot == LOOT            // último foi loot normal (saquinho)
			 && last_success.x > 0                  // tem posição válida
			 && last_success.y > 0)
	 {
		loga($"Início inteligente: testando last_success ({last_success.x},{last_success.y})");

		mousemove(last_success.x, last_success.y);
		wait(pausa);
		byte tipo = tipocur();

		if (tipo == SKIN && cb_skinning.Checked) // encontrou skin na posição anterior!
		{
		 ultimo_tipo_loot = SKIN;              // atualiza tipo do último loot
		 total_loots++;
		 logastats();
		 lootCombates++;
		 loga("Extraindo couro (início inteligente).");
		 return last_success;                  // retorna a mesma posição
		}
		else
		{
		 loga("Início inteligente: nada encontrado na posição anterior. Fazendo scan normal.");
		}
	 }

	 // --------------------------------
	 // SCAN NORMAL: TENTA PONTO CENTRAL
	 // --------------------------------
	 loc centro = new loc { x = cx, y = cy };
	 mousemove(centro.x, centro.y); wait(pausa);
	 byte tipo_centro = tipocur(); // obtém tipo do cursor atual

	 if (tipo_centro == LOOT || (tipo_centro == SKIN && cb_skinning.Checked) || (tipo_centro == HERB && catando_planta))
	 {
		last_success = centro;
		ultimo_tipo_loot = tipo_centro;           // salva tipo encontrado
		primeira_tentativa_sessao = false;       // marca que já fez primeira tentativa
		total_loots++;
		logastats();
		lootCombates++;

		if (tipo_centro == LOOT) loga("Realizando loot.");
		else if (tipo_centro == SKIN) loga("Extraindo couro.");
		else if (tipo_centro == HERB) loga("Localizou planta.");

		return centro;
	 }

	 // --------------------------------
	 // SCAN NORMAL: ORDENA POSIÇÕES 1 a 24 POR FREQUÊNCIA
	 // --------------------------------
	 List<int> ordem = Enumerable.Range(1, 24)
			 .OrderByDescending(i => lootfreq[i])
			 .ThenBy(i => i)
			 .ToList();

	 Random rnd = new Random();

	 foreach (int idx in ordem)
	 {
		// chance de pular ponto com freq 0
		if (lootfreq[idx] == 0 && !catando_planta && rnd.NextDouble() < CHANCE_PULAR_ZERO_FREQ)
		 continue;

		// calcula ângulo em radianos
		double ang = Math.PI * (idx - 1) / (idx <= 16 ? 8.0 : 4.0);

		// define raio conforme idx
		int r = (int)(h * (idx <= 16 ? 0.20 : 0.10));

		// calcula posição
		loc p = new loc
		{
		 x = cx + (int)(Math.Cos(ang) * r),
		 y = cy - (int)(Math.Sin(ang) * r)
		};

		mousemove(p.x, p.y); wait(pausa);
		byte tipo = tipocur(); // lê tipo do cursor nesse ponto

		if (tipo == LOOT || (tipo == SKIN && cb_skinning.Checked) || (tipo == HERB && catando_planta))
		{
		 atualiza_lootfreq(idx); // atualiza frequência (exceto centro)
		 last_success = p;
		 ultimo_tipo_loot = tipo;              // salva tipo encontrado
		 primeira_tentativa_sessao = false;   // marca que já fez primeira tentativa
		 total_loots++;
		 logastats();
		 lootCombates++;

		 if (tipo == LOOT) loga("Realizando loot.");
		 else if (tipo == SKIN) loga("Extraindo couro.");
		 else if (tipo == HERB) loga("Localizou planta.");

		 return p; // retorna a coordenada onde encontrou
		}
	 }

	 // --------------------------------
	 // NADA ENCONTRADO
	 // --------------------------------
	 return new loc { x = -1, y = -1 }; // código de erro
	}


	Queue<int> fila_fifo = new Queue<int>(); // fora do método, persistente


	// ----------------------------------------  
	// FUNÇÃO carrega_loot  
	// Lê loot.txt, inicializa lootfreq[] e fila_fifo  
	// Ignora valores negativos e posição 0 da fila  
	// Zera lootfreq[0] por segurança e oculta no log  
	// ----------------------------------------
	void carrega_loot()
	{
	 string arq = "loot.txt";

	 if (!File.Exists(arq)) // cria padrão se não existe
	 {
		List<string> linhas = new List<string>();
		linhas.Add("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"); // lootfreq zerada
		linhas.Add(""); // fila_fifo vazia
		File.WriteAllLines(arq, linhas);
		loga("loot.txt não encontrado. Criado arquivo padrão.");
	 }

	 string[] dados = File.ReadAllLines(arq);
	 if (dados.Length < 2) return;

	 // carrega lootfreq[25] da primeira linha
	 string[] partes1 = dados[0].Split(',');
	 for (int i = 0; i < partes1.Length && i < 25; i++)
	 {
		if (int.TryParse(partes1[i].Trim(), out int val))
		 lootfreq[i] = Math.Max(0, val); // evita negativos
		else
		 lootfreq[i] = 0; // fallback
	 }

	 lootfreq[0] = 0; // zera posição 0 por segurança

	 // reconstrói fila_fifo com os índices da segunda linha (ignora pos 0)
	 fila_fifo.Clear();
	 string[] partes2 = dados[1].Split(',');
	 foreach (string s in partes2)
	 {
		if (int.TryParse(s.Trim(), out int idx) && idx > 0 && idx < 25)
		 fila_fifo.Enqueue(idx);
	 }

	 // atualiza total_loots como soma da fila
	 total_loots = fila_fifo.Count;
	 loga($"lootfreq carregada: {string.Join(",", lootfreq.Skip(1))}"); // ignora posição 0
	 loga($"total_loots atualizado: {total_loots}");
	}




	// ----------------------------------------
	// FUNÇÃO ATUALIZA LOOTFREQ (janela FIFO adaptativa)
	// Ignora posição 0 (fixa); mantém apenas índice 1 a 24
	// ----------------------------------------
	void atualiza_lootfreq(int pos)
	{
	 if (pos <= 0 || pos >= 25) return; // ignora índice 0 e inválidos

	 if (fila_fifo.Count >= 1000)
	 {
		int velho = fila_fifo.Dequeue();
		if (velho > 0 && velho < 25)
		 lootfreq[velho] = Math.Max(0, lootfreq[velho] - 1); // evita negativo
	 }

	 fila_fifo.Enqueue(pos);
	 lootfreq[pos]++;
	}

	// ----------------------------------------
	// FUNÇÃO salva_loot
	// Salva vetor lootfreq[1 a 24] e a fila_fifo no loot.txt
	// Aplica trim leve se algum valor passar de 400
	// ----------------------------------------
	void salva_loot()
	{
	 string arq = "loot.txt";

	 // aplica trim leve se necessário (apenas índices 1 a 24)
	 int max = lootfreq.Skip(1).Take(24).Max();
	 if (max > 2000)
	 {
		double fator = 2000.0 / max;
		for (int i = 1; i < 25; i++)
		 lootfreq[i] = Math.Max(0, (int)(lootfreq[i] * fator)); // trim e clipe
	 }

	 // clipe final de segurança
	 for (int i = 1; i < 25; i++)
		lootfreq[i] = Math.Max(0, lootfreq[i]);

	 // monta linha 1: lootfreq[1..24] apenas
	 string linha1 = string.Join(",", lootfreq.Skip(1).Take(24));

	 // monta linha 2: fila_fifo
	 string linha2 = string.Join(",", fila_fifo);

	 // grava arquivo com duas linhas
	 File.WriteAllLines(arq, new[] { linha1, linha2 });

	 // loga salvamento
	 loga($"loot salvo: total_loots={fila_fifo.Count}, max_freq={max}");
	}



	// --------------------------------------------
	// EVENTO DO BOTÃO button7: SIMULAÇÃO SÍNCRONA DE OTIMIZAÇÃO
	// --------------------------------------------
	// Executa a simulação direto no thread principal, sem async/await
	// Log inicial e final para acompanhar o processo
	// evento do botão chama o método
	private void button7_Click(object sender, EventArgs e)
	{
	 find_center(); // chama o método de encontrar o centro do minimapa
	}

	// Adicionar próximo às outras definições de classe, geralmente no início do arquivo
	public class Cerca
	{
	 public loc pontoA { get; set; }   // extremidade A da cerca
	 public loc pontoB { get; set; }   // extremidade B da cerca
	 public loc pontoP { get; set; }   // ponto que define lado proibido

	 // Construtor para facilitar criação
	 public Cerca(loc a, loc b, loc p)
	 {
		pontoA = a;
		pontoB = b;
		pontoP = p;
	 }

	 // Método para converter para string (para salvar no arquivo)
	 public string ToPacked()
	 {
		return $"{pack(pontoA)},{pack(pontoB)},{pack(pontoP)}";
	 }

	 // Método estático para criar Cerca a partir de uma string packed
	 public static Cerca FromPacked(string packed)
	 {
		string[] coords = packed.Split(',');
		if (coords.Length != 12)
		 throw new ArgumentException("Formato de cerca inválido");

		// Reconstrói os três pontos
		loc a = new loc(
				int.Parse(coords[0] + coords[1]),
				int.Parse(coords[2] + coords[3])
		);
		loc b = new loc(
				int.Parse(coords[4] + coords[5]),
				int.Parse(coords[6] + coords[7])
		);
		loc p = new loc(
				int.Parse(coords[8] + coords[9]),
				int.Parse(coords[10] + coords[11])
		);

		return new Cerca(a, b, p);
	 }
	}



	// --------------------------------
	// BOTÃO SALVAR WAYPOINTS
	// --------------------------------
	private void bt_saveWP_Click(object sender, EventArgs e)
	{
	 if (lbwp.Items.Count > 0)
	 {
		List<string> linhas = new List<string>();

		// Primeiro salva waypoints
		foreach (var item in lbwp.Items)
		{
		 string linha = item.ToString();
		 int virgulas = linha.Count(c => c == ',');

		 if (virgulas == 3)  // Waypoint (4 números)
			linhas.Add(linha);
		}

		// Depois salva cercas
		foreach (Cerca cerca in lcercas)
		{
		 linhas.Add(cerca.ToPacked());
		}

		string nome = tb_filename.Text.Trim();
		if (nome == "") nome = "waypoints.txt";
		else if (!nome.Contains(".")) nome += ".txt";

		File.WriteAllLines(nome, linhas);        // salva os waypoints
		File.WriteAllText("discord.ini", nome);  // grava só o nome no cfg

		if (true)
		{
		 loga($"Waypoints salvos:");
		 loga($"  Waypoints navegáveis: {lwp.Count}");
		 loga($"  Cercas preservadas: {lcercas.Count}");
		 loga($"Arquivo: {nome}");
		}
	 }
	 else
	 {
		loga("Nenhum waypoint para salvar");
	 }
	}

	// ----------------------------------------
	// MÉTODO checksum
	// Gera uma assinatura leve de 1 byte (0–255)
	// Soma todos os bytes da imagem e aplica mod 256
	// ----------------------------------------
	public byte checksum(byte[] data)
	{
	 int soma = 0;

	 for (int i = 0; i < data.Length; i++)
		soma += data[i]; // soma simples

	 return (byte)(soma % 256); // resultado final reduzido para 1 byte
	}





	// --------------------------------
	// EVENTO AO MARCAR OU DESMARCAR CB_ANDAR
	// --------------------------------
	private void cb_anda_CheckedChanged(object sender, EventArgs e)
{
if (!cb_anda.Checked)
{
 if (on) // se estava ligado
 {
 loga("cb_anda desmarcado: parando o bot.");
	on = false; // para o loop principal
 }
}
else
{
 if (!on) // só se o bot estava parado
 {
	loga("cb_anda marcado: reiniciando o bot.");
	on = true; // liga o bot
	button1_Click_1(null, null); // chama o botão StartBot
 }
}
}

	// Método manual de clamp, substitui Math.Clamp para compatibilidade
	float Clamp(float value, float min, float max)
	{
	 return Math.Min(Math.Max(value, min), max);
	}


	//-------------------------
	// CHECKFENCE - VERIFICA SE ESTOU DO LADO ERRADO DE ALGUMA CERCA
	//-------------------------
	// Método para calcular distância ponto-linha
	float DistanciaPointToLineSegment(loc ponto, loc lineStart, loc lineEnd)
	{
	 float lineMagnitude = dist(lineStart, lineEnd);

	 // Se a linha tem comprimento zero, retorna distância do ponto ao ponto inicial
	 if (lineMagnitude == 0)
		return dist(ponto, lineStart);

	 // Projeção do vetor
	 float t = ((ponto.x - lineStart.x) * (lineEnd.x - lineStart.x) +
							(ponto.y - lineStart.y) * (lineEnd.y - lineStart.y)) / (lineMagnitude * lineMagnitude);

	 // Clamp entre 0 e 1
	 t = Math.Max(0, Math.Min(1, t));

	 // Ponto de projeção
	 loc projecao = new loc(
			 (int)Math.Round(lineStart.x + t * (lineEnd.x - lineStart.x)),
			 (int)Math.Round(lineStart.y + t * (lineEnd.y - lineStart.y))
	 );

	 // Retorna distância
	 return dist(ponto, projecao);
	}

	// Método para determinar lado do ponto em relação à linha
	bool IsPointOnProhibitedSide(loc ponto, loc lineStart, loc lineEnd, loc proibidoPoint)
	{
	 // Produto vetorial para determinar lado
	 int crossProduct = (lineEnd.x - lineStart.x) * (ponto.y - lineStart.y) -
											(lineEnd.y - lineStart.y) * (ponto.x - lineStart.x);

	 // Produto vetorial para lado proibido
	 int crossProductProibido = (lineEnd.x - lineStart.x) * (proibidoPoint.y - lineStart.y) -
															 (lineEnd.y - lineStart.y) * (proibidoPoint.x - lineStart.x);

	 // Se sinais são diferentes, está no lado proibido
	 return (crossProduct * crossProductProibido < 0);
	}

	// Método principal de verificação de cerca
	bool checkfence()
	{
	 if (lcercas == null || lcercas.Count == 0)
		return false;

	 foreach (Cerca cerca in lcercas)
	 {
		// Verifica distância da cerca
		float distancia = DistanciaPointToLineSegment(me.pos, cerca.pontoA, cerca.pontoB);

		// Só avalia cercas próximas (dentro de 100 unidades)
		if (distancia <= 100)
		{
		 // Verifica se está no lado proibido
		 if (IsPointOnProhibitedSide(me.pos, cerca.pontoA, cerca.pontoB, cerca.pontoP))
		 {
			loga($"🚨 VIOLAÇÃO DE CERCA: Distância {distancia:F2}");
			return true;
		 }
		}
	 }

	 return false;
	}


	// DRAW MAP
	// --------------------------------------------
	// MÉTODO desenhamapa - Gera imagem do mapa com o player e o alvo
	// --------------------------------------------
	void drawmap(loc alvo)
	{
	 checkme();                         // atualiza posição do player
	 points.Clear();                    // limpa lista de pontos
	 points.Add(new point(me.pos, 1));  // adiciona player (vermelho)
	 points.Add(new point(alvo, 4));    // adiciona alvo (amarelo)

	 // Tenta converter o valor da textbox, com fallback para 100
	 float zoom = 100f;
	 if (float.TryParse(tb_map_zoom.Text, out float zoom_input))
	 {
		zoom = Clamp(zoom_input, 20f, 100f);  // limita entre 50 e 300
	 }

	 float esc = 1.5f * (zoom / 100f);  // escala gráfica
	 Bitmap bmp = new Bitmap(256, 256);
	 using (Graphics g = Graphics.FromImage(bmp))
	 {
		g.Clear(Color.Black); // fundo preto
		foreach (point p in points)
		{
		 int dxi = (int)((p.pos.x - me.pos.x) * esc);
		 int dyi = (int)((p.pos.y - me.pos.y) * esc);
		 int px = 128 + dxi;
		 int py = 128 + dyi;
		 if (px >= 0 && px < 256 && py >= 0 && py < 256)
		 {
			Brush b = Brushes.White;
			if (p.val == 1) b = Brushes.Red;
			if (p.val == 2) b = Brushes.Blue;
			if (p.val == 3) b = Brushes.Lime;
			if (p.val == 4) b = Brushes.Yellow;
			g.FillRectangle(b, px, py, 2, 2);
		 }
		}

		// seta branca da direção atual
		double rad = me.facing * Math.PI / 180.0;
		int fx = 128 - (int)(Math.Sin(rad) * 20);
		int fy = 128 - (int)(Math.Cos(rad) * 20);
		g.DrawLine(Pens.White, 128, 128, fx, fy);

		// linha verde tracejada até o alvo
		using (Pen penTracejada = new Pen(Color.Green, 1))
		{
		 penTracejada.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
		 int tx = 128 + (int)((alvo.x - me.pos.x) * esc);
		 int ty = 128 + (int)((alvo.y - me.pos.y) * esc);
		 double dx = tx - 128;
		 double dy = 128 - ty;
		 double ang = Math.Atan2(dx, dy) * (180.0 / Math.PI);
		 if (ang < 0) ang += 360;
		 int yaw_visual = (int)Math.Round(ang);
		 lb_delta.Text = getyaw(me.pos, alvo).ToString();   // yaw real
		 g.DrawLine(penTracejada, 128, 128, tx, ty);
		}

		// Adiciona desenho de cercas
		if (lcercas != null && lcercas.Count > 0)
		{
		 foreach (Cerca cerca in lcercas)
		 {
			// Converte coordenadas para posição no mapa
			int dxA = (int)((cerca.pontoA.x - me.pos.x) * esc);
			int dyA = (int)((cerca.pontoA.y - me.pos.y) * esc);
			int dxB = (int)((cerca.pontoB.x - me.pos.x) * esc);
			int dyB = (int)((cerca.pontoB.y - me.pos.y) * esc);
			int dxP = (int)((cerca.pontoP.x - me.pos.x) * esc);
			int dyP = (int)((cerca.pontoP.y - me.pos.y) * esc);

			int pxA = 128 + dxA;
			int pyA = 128 + dyA;
			int pxB = 128 + dxB;
			int pyB = 128 + dyB;
			int pxP = 128 + dxP;
			int pyP = 128 + dyP;

			// Desenha linha da cerca em vermelho
			using (Pen redPen = new Pen(Color.Red, 1))
			{
			 g.DrawLine(redPen, pxA, pyA, pxB, pyB);
			}

			// Marca ponto proibido com um X
			using (Pen bluePen = new Pen(Color.Blue, 1))
			{
			 g.DrawLine(bluePen, pxP - 2, pyP - 2, pxP + 2, pyP + 2);
			 g.DrawLine(bluePen, pxP - 2, pyP + 2, pxP + 2, pyP - 2);
			}
		 }
		}
	 }
	 pb_map.Image = bmp; // exibe na picturebox
	}


	// --------------------------------
	// SAVE CONFIG (salva todas as checkbox e textbox) 
	// --------------------------------
	// ---------------------------------------------
	// BOTÃO BT_SAVE_CFG - Salva status dos CheckBox e TextBox
	// ---------------------------------------------
	private void bt_save_cfg_Click(object sender, EventArgs e)
	{
	 salva_loot();                   // salva loot.txt, frequencias de loot
	 prox_salva = DateTime.Now.AddMinutes(10); // reinicia o timer

	 List<string> linhas = new List<string>(); // lista de saída

	 coleta_controles(this, linhas); // varre todos os controles do Form recursivamente

	 linhas.Add("win_left=" + this.Left);      // posição X da janela
	 linhas.Add("win_top=" + this.Top);        // posição Y da janela
	 linhas.Add("win_width=" + this.Width);    // largura (opcional)
	 linhas.Add("win_height=" + this.Height);  // altura (opcional)
	 linhas.Add("win_monitor=" + Screen.FromControl(this).DeviceName); // nome do monitor

	 File.WriteAllLines("discord.cfg", linhas); // grava o arquivo
	}



	private void button8_Click(object sender, EventArgs e)
	{
	 lb_log.Text = ""; // limpa
	}
	// BOTAO COMBAT ONLY
	bool dungeon = false; // variável para dungeon mode'
	private void button12_Click(object sender, EventArgs e)
	{
	 loga("Modo combate estático / dungeon ativado.");
	 on = true;
	 dungeon = true; // ativa modo dungeon
	 cb_anda.Checked = false;
	 on = true;
	 while (on)
	 {
		checkme();
		if (me.combat)
		 combatloop(); // chama o loop de combate
		wait(200);
	 }
	 loga("Modo combate desativado.");
	 dungeon = false; // desativa modo dungeon
	 cb_anda.Checked = true; // reativa andar normal
	 on = true; // reativa o bot

	}

	// --------------------------------
	// BOTÃO CARREGAR WAYPOINTS MANUALMENTE
	// --------------------------------
	private void bt_loadWP_Click(object sender, EventArgs e)
	{
	 using (OpenFileDialog ofd = new OpenFileDialog())
	 {
		ofd.InitialDirectory = Application.StartupPath; // pasta do executável
		ofd.Filter = "Arquivos de texto (*.txt)|*.txt|Todos os arquivos (*.*)|*.*";
		ofd.Title = "Selecione um arquivo de waypoints";

		if (ofd.ShowDialog() == DialogResult.OK)
		{
		 // Atualiza o nome do arquivo
		 tb_filename.Text = Path.GetFileName(ofd.FileName);

		 // Atualiza o discord.cfg com o novo nome
		 File.WriteAllText("discord.ini", tb_filename.Text.Trim());

		 // Chama o método de carregamento atualizado
		 carregar_waypoints();
		}
	 }
	}

	// --------------------------------
	// BOTÃO SALVAR COMO... (SAVE AS)
	// --------------------------------
	private void bt_saveWPas_Click(object sender, EventArgs e)
	{
	 using (SaveFileDialog sfd = new SaveFileDialog())
	 {
		sfd.InitialDirectory = Application.StartupPath; // pasta do executável
		sfd.Filter = "Arquivos de texto (*.txt)|*.txt";
		sfd.Title = "Salvar waypoints como...";
		sfd.FileName = tb_filename.Text.Trim(); // sugere o nome atual

		if (sfd.ShowDialog() == DialogResult.OK)
		{
		 string nomeCompleto = sfd.FileName;
		 string nomeSimples = Path.GetFileName(nomeCompleto); // só o nome, sem path

		 tb_filename.Text = nomeSimples; // atualiza textbox
		 File.WriteAllText("discord.ini", nomeSimples); // salva nome no cfg

		 bt_saveWP.PerformClick(); // chama o botão de salvar
		}
	 }
	}
	// BOTAO STOP
	private void button9_Click(object sender, EventArgs e)
	{
	 on = false;
	}
	// ----------------------------------------
	// BOTAO REMOVE WAYPOINT
	// remove o waypoint selecionado da listbox e da lista
	// ----------------------------------------
	private void button11_Click(object sender, EventArgs e)
	{
	 if (lbwp.SelectedIndex == -1)
	 {
		loga("Nenhum waypoint selecionado para remover.");
		return;
	 }

	 int i = lbwp.SelectedIndex;
	 lbwp.Items.RemoveAt(i); // remove da listbox

	 // reconstrói lista real baseada na listbox
	 lwp.Clear(); // limpa lista virtual
	 foreach (string item in lbwp.Items)
	 {
		loc p = unpack(item); // desempacota string de volta pra loc
		lwp.Add(p);           // adiciona na lista real
	 }

	 loga("Waypoint removido e lista atualizada.");
	}
	// ----------------------------------------------
	// VARS GLOBAIS DO MAPA
	// ----------------------------------------------
	byte[,] r, g, b; // canais do mapa
	int w = 1000, h = 1000; // dimensões fixas
	string path = ""; // caminho do bmp atual
	Bitmap bmp; // imagem atual




							// ----------------------------------------------
							// MÉTODO LEMAPA - CARREGA MAPA DO DISCO PARA OS ARRAYS r, g, b
							// ----------------------------------------------
	void lemapa(string nome)
	{
	 if (string.IsNullOrWhiteSpace(nome)) return; // ignora nome vazio
	 string pasta = "mapas"; // pasta destino
	 if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta); // cria se não existir
	 path = Path.Combine(pasta, nome + ".bmp"); // caminho completo

	 w = 1000;
	 h = 1000;
	 r = new byte[w, h];
	 g = new byte[w, h];
	 b = new byte[w, h];

	 if (!File.Exists(path)) return; // se não existe, sai
	 bmp = new Bitmap(path); // carrega o bitmap do disco
	 Rectangle ret = new Rectangle(0, 0, w, h);
	 BitmapData data = bmp.LockBits(ret, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
	 IntPtr ptr = data.Scan0;
	 int stride = data.Stride;
	 byte[] raw = new byte[stride * h];
	 Marshal.Copy(ptr, raw, 0, raw.Length);

	 for (int y = 0; y < h; y++)
		for (int x = 0; x < w; x++)
		{
		 int i = y * stride + x * 3;
		 b[x, y] = raw[i];     // azul
		 g[x, y] = raw[i + 1]; // verde
		 r[x, y] = raw[i + 2]; // vermelho
		}

	 bmp.UnlockBits(data);
	 bmp.Dispose(); // libera o handle
	 bmp = null;
	}





	// ----------------------------------------------
	// BOTAO LER MAPA - CARREGA OU CRIA MAPA NO DISCO
	// ----------------------------------------------
	private void button13_Click(object sender, EventArgs e)
	{
	 cb_atualiza_mapa.Checked = true;

	 string nome = tb_mapname.Text.Trim(); // lê nome do mapa
	 if (nome == "") return; // ignora se estiver vazio

	 string pasta = "mapas"; // pasta destino
	 if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta); // cria pasta se não existir

	 path = Path.Combine(pasta, nome + ".bmp"); // caminho do arquivo

	 w = 1000;
	 h = 1000;

	 // se não existir, cria o mapa em preto
	 if (!File.Exists(path))
	 {
		Bitmap novo = new Bitmap(w, h, PixelFormat.Format24bppRgb);
		for (int y = 0; y < h; y++)
		 for (int x = 0; x < w; x++)
			novo.SetPixel(x, y, Color.Black);
		novo.Save(path);
		novo.Dispose();
	 }

	 lemapa(nome); // carrega o mapa nos arrays r,g,b
	}

	// ----------------------------------------------
	// MÉTODO DESENHAMAPA_FOCAL - UNE PONTOS PRÓXIMOS NUMA JANELA AO REDOR DE UM PONTO
	// ----------------------------------------------
	DateTime ultimo_salvo = DateTime.Now; // controle de tempo

	void atualizamapa(loc pos)
	{
	 if (!cb_atualiza_mapa.Checked)
		return;

	 int cx = pos.x / 10;
	 int cy = pos.y / 10;

	 int range = 20; // raio de varredura
	 int? lx = null, ly = null; // último ponto encontrado

	 for (int y = Math.Max(0, cy - range); y <= Math.Min(h - 1, cy + range); y++)
		for (int x = Math.Max(0, cx - range); x <= Math.Min(w - 1, cx + range); x++)
		{
		 if (r[x, y] == 255 && g[x, y] == 255 && b[x, y] == 255)
		 {
			if (lx != null && ly != null)
			{
			 int dx = x - lx.Value;
			 int dy = y - ly.Value;
			 double dist = Math.Sqrt(dx * dx + dy * dy);

			 if (dist <= 20)
			 {
				int passos = (int)Math.Max(Math.Abs(dx), Math.Abs(dy));
				for (int i = 0; i <= passos; i++)
				{
				 int xi = lx.Value + dx * i / passos;
				 int yi = ly.Value + dy * i / passos;

				 if (xi >= 0 && xi < w && yi >= 0 && yi < h)
				 {
					r[xi, yi] = 255;
					g[xi, yi] = 255;
					b[xi, yi] = 255;
				 }
				}
			 }
			}

			lx = x;
			ly = y;
		 }
		}

	 // salva no disco a cada 30 segundos
	 if ((DateTime.Now - ultimo_salvo).TotalSeconds >= 30)
	 {
		loga("Salvando mapa.");
		Bitmap bmp2 = new Bitmap(w, h, PixelFormat.Format24bppRgb);
		Rectangle ret = new Rectangle(0, 0, w, h);
		BitmapData data = bmp2.LockBits(ret, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
		IntPtr ptr = data.Scan0;
		int stride = data.Stride;
		byte[] raw = new byte[stride * h];

		for (int y = 0; y < h; y++)
		 for (int x = 0; x < w; x++)
		 {
			int i = y * stride + x * 3;
			raw[i] = b[x, y];
			raw[i + 1] = g[x, y];
			raw[i + 2] = r[x, y];
		 }

		Marshal.Copy(raw, 0, ptr, raw.Length);
		bmp2.UnlockBits(data);
		bmp2.Save(path);
		bmp2.Dispose();
		ultimo_salvo = DateTime.Now;
	 }
	}

	private void Form1_Load(object sender, EventArgs e)
	{
	 
	}



	private void button10_Click(object sender, EventArgs e)
	{
	 if (pb_minimap.Image == null)
	 {
		loga("Nenhuma imagem carregada no pb_minimap.");
		return;
	 }

	 // converte para bitmap e salva como arquivo .bmp
	 Bitmap bmp = new Bitmap(pb_minimap.Image);
	 bmp.Save("bmp.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
	 loga("Imagem salva como bmp.bmp");
	}

	private void pb_minimap_Click(object sender, MouseEventArgs e)
	{
	 if (pb_minimap.Image == null) return;

	 // pega o bitmap real
	 Bitmap bmp = (Bitmap)pb_minimap.Image;

	 // pega tamanho real da imagem e do picturebox
	 int bmp_w = bmp.Width;
	 int bmp_h = bmp.Height;
	 int box_w = pb_minimap.Width;
	 int box_h = pb_minimap.Height;

	 // calcula escala entre a imagem real e o PictureBox
	 float escala_x = bmp_w / (float)box_w;
	 float escala_y = bmp_h / (float)box_h;

	 // converte clique para coordenadas da imagem
	 int px = (int)(e.X * escala_x);
	 int py = (int)(e.Y * escala_y);

	 loga($"Clique em {e.X},{e.Y} → imagem: {px},{py}");

	 // opcional: pega cor do pixel clicado
	 Color c = bmp.GetPixel(px, py);
	 loga($"Cor do pixel: R={c.R} G={c.G} B={c.B}");
	}

	private void pb_minimap_MouseClick(object sender, MouseEventArgs e)
	{
	 if (pb_minimap.Image == null) return;
	 Bitmap bmp = (Bitmap)pb_minimap.Image;

	 int bmp_w = bmp.Width;
	 int bmp_h = bmp.Height;
	 int box_w = pb_minimap.Width;
	 int box_h = pb_minimap.Height;

	 // converte clique visual para posição real no bitmap
	 float escala_x = bmp_w / (float)box_w;
	 float escala_y = bmp_h / (float)box_h;
	 int px = (int)(e.X * escala_x);
	 int py = (int)(e.Y * escala_y);

	 loga($"Clique em {e.X},{e.Y} → imagem: {px},{py}");

	 Color c = bmp.GetPixel(px, py);
	 loga($"Cor do pixel: R={c.R} G={c.G} B={c.B}");

	 // ---------------------------------------------
	 // CÁLCULO DE POSIÇÃO ESTIMADA NO MUNDO
	 // ---------------------------------------------
	 int cx = 60; // centro fixo da flecha
	 int cy = 65;

	 int dx = px - cx;
	 int dy = py - cy;

	 float fator_x = 4.53f;
	 float fator_y = 6.85f;

	 float gx = me.pos.x + dx * fator_x;
	 float gy = me.pos.y + dy * fator_y;

	 float gx_real = gx / 100f;
	 float gy_real = gy / 100f;

	 loga($"Estimado no mapa: {gx_real:0.00}, {gy_real:0.00}");
	}


	private void pb_minimap_Click(object sender, EventArgs e)
	{

	}


	 private void bt_loopminimap_Click(object sender, EventArgs e)
	{
	 while (on)
	 {
		int tela_w = Screen.PrimaryScreen.Bounds.Width;

		int dx = int.Parse(tbx.Text);
		int dy = int.Parse(tby.Text);
		int raio = int.Parse(tbraio.Text);

		int cx = tela_w - dx;
		int cy = dy;
		int lado = raio * 2;

		int x = cx - raio;
		int y = cy - raio;

		Bitmap bmp = new Bitmap(lado, lado);
		using (Graphics g = Graphics.FromImage(bmp))
		{
		 g.CopyFromScreen(x, y, 0, 0, new Size(lado, lado));
		}

		pb_minimap.Image = bmp;
		loga("Loop minimap capturado");

		wait(500); // pausa entre capturas

		if (!on) break;
	 }
	



	}


	private void button14_Click(object sender, EventArgs e)
	{

	}


	// ----------------------------------------
	// MÉTODO getpixels
	// Extrai os bytes do HBITMAP renderizado
	// Retorna um array de 32*32*4 = 4096 bytes (BGRA)
	// ----------------------------------------
	public byte[] getpixels(IntPtr hBitmap)
	{
	 const int width = 32;
	 const int height = 32;
	 const int bytesPerPixel = 4;
	 const int totalBytes = width * height * bytesPerPixel;

	 byte[] pixels = new byte[totalBytes]; // buffer de destino

	 BITMAPINFOHEADER bih = new BITMAPINFOHEADER();
	 bih.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
	 bih.biWidth = width;
	 bih.biHeight = -height; // negativo para top-down
	 bih.biPlanes = 1;
	 bih.biBitCount = 32;
	 bih.biCompression = 0; // BI_RGB

	 IntPtr hdc = GetDC(IntPtr.Zero); // DC da tela

	 GetDIBits(hdc, hBitmap, 0, (uint)height, pixels, ref bih, 0);

	 ReleaseDC(IntPtr.Zero, hdc); // libera DC
	 return pixels;
	}



	// --------------------------------------------
	// MÉTODO FIND_PLANTS: LOCALIZA PLANTAS NO MINIMAPA
	// --------------------------------------------
	loc find_plants()
	{
	 checkme(); // atualiza posição do player
	 if (pb_minimap.Image == null) return new loc(); // se imagem ausente, retorna vazio

	 Bitmap bmp = (Bitmap)pb_minimap.Image;
	 int cx = 60;                        // centro da seta no bitmap
	 int cy = 65;

	 float fator_x = 4.53f;              // fator de conversão horizontal
	 float fator_y = 6.85f;              // fator de conversão vertical

	 int lim_r = 228, lim_g = 175, lim_b = 5;
	 int lim_r2 = 231, lim_g2 = 190, lim_b2 = 40;
	 int dist_max = 6;

	 List<List<Point>> grupos = new List<List<Point>>();

	 for (int y = 0; y < bmp.Height; y++)
	 {
		for (int x = 0; x < bmp.Width; x++)
		{
		 Color c = bmp.GetPixel(x, y);

		 if (c.R >= lim_r && c.R <= lim_r2 &&
			 c.G >= lim_g && c.G <= lim_g2 &&
			 c.B >= lim_b && c.B <= lim_b2)
		 {
			Point p = new Point(x, y);
			bool agrupado = false;

			for (int i = 0; i < grupos.Count; i++)
			{
			 foreach (Point q in grupos[i])
			 {
				int dx = p.X - q.X;
				int dy = p.Y - q.Y;
				if (dx * dx + dy * dy <= dist_max * dist_max)
				{
				 grupos[i].Add(p);
				 agrupado = true;
				 break;
				}
			 }
			 if (agrupado) break;
			}

			if (!agrupado)
			{
			 List<Point> novo = new List<Point>();
			 novo.Add(p);
			 grupos.Add(novo);
			}
		 }
		}
	 }

	 List<loc> plantas = new List<loc>();

	 foreach (var grupo in grupos)
	 {
		bool tem_nucleo = false;
		int soma_x = 0;
		int soma_y = 0;

		foreach (Point p in grupo)
		{
		 soma_x += p.X;
		 soma_y += p.Y;

		 Color c = bmp.GetPixel(p.X, p.Y);
		 if (c.B <= 12) tem_nucleo = true;
		}

		if (!tem_nucleo) continue;

		int mx = soma_x / grupo.Count;
		int my = soma_y / grupo.Count;

		int dx = mx - cx;
		int dy = my - cy;

		float gx = me.pos.x + dx * fator_x;
		float gy = me.pos.y + dy * fator_y;

		float gx_real = gx / 100f;
		float gy_real = gy / 100f;

		loga($"Planta em: {gx_real:0.00}, {gy_real:0.00}");

		loc l = new loc();
		l.x = (int)(gx_real * 100); // armazenado em centésimos
		l.y = (int)(gy_real * 100);
		plantas.Add(l);
	 }

	 if (plantas.Count == 0)
	 {
		//loga("Nenhuma planta encontrada.");
		return new loc();
	 }

	 // escolhe a mais próxima da posição atual
	 loc melhor = plantas[0];
	 int menor_dist = int.MaxValue;

	 foreach (loc l in plantas)
	 {
		int dx = l.x - me.pos.x;
		int dy = l.y - me.pos.y;
		int dist = dx * dx + dy * dy;

		if (dist < menor_dist)
		{
		 melhor = l;
		 menor_dist = dist;
		}
	 }

	 return melhor;
	}

	// --------------------------------------------
	// BOTÃO FIND_DOTS: LOCALIZA UMA PLANTA
	// --------------------------------------------
	private void find_dots_Click(object sender, EventArgs e)
	{
	 loc alvo = find_plants(); // pega o mais próximo
	 tb_debug1.Text = alvo.x.ToString(); // mostra X em centésimos
	 tb_debug2.Text = alvo.y.ToString(); // mostra Y em centésimos
	}



	// --------------------------------------------
	// MÉTODO GET_MINIMAP: CAPTURA O MINIMAPA ATUAL
	// --------------------------------------------
	void get_minimap()
	{
	 int tela_w = Screen.PrimaryScreen.Bounds.Width; // largura da tela principal

	 int dx = int.Parse(tbx.Text);         // distância da borda direita
	 int dy = int.Parse(tby.Text);         // distância do topo
	 int raio = int.Parse(tbraio.Text);    // raio da captura

	 int cx = tela_w - dx;                 // coordenada central X do minimapa
	 int cy = dy;                          // coordenada central Y
	 int lado = raio * 2;                  // lado total do quadrado

	 int x = cx - raio;                    // canto superior esquerdo X
	 int y = cy - raio;                    // canto superior esquerdo Y

	 Bitmap bmp = new Bitmap(lado, lado);  // cria novo bitmap
	 using (Graphics g = Graphics.FromImage(bmp))
	 {
		g.CopyFromScreen(x, y, 0, 0, new Size(lado, lado)); // captura da tela
	 }

	 pb_minimap.Image = bmp;               // exibe imagem capturada na picturebox
	}


	// --------------------------------------------
	// MÉTODO FIND_CENTER: LOCALIZA A SETA DO PLAYER
	// --------------------------------------------
	void find_center()
	{
	 if (pb_minimap.Image == null) return; // cancela se não tiver imagem

	 Bitmap bmp = (Bitmap)pb_minimap.Image; // pega bitmap atual do minimapa
	 int w = bmp.Width;                     // largura do bitmap
	 int h = bmp.Height;                    // altura do bitmap

	 List<Point> pontos = new List<Point>(); // armazena os pixels válidos

	 for (int y = 0; y < h; y++) // varre linhas do bitmap
	 {
		for (int x = 0; x < w; x++) // varre colunas
		{
		 Color c = bmp.GetPixel(x, y); // pega cor do pixel

		 // verifica se é um tom claro de cinza
		 if (c.R >= 230 && c.G >= 230 && c.B >= 230)
			if (Math.Abs(c.R - c.G) <= 6 && Math.Abs(c.G - c.B) <= 6 && Math.Abs(c.R - c.B) <= 6)
			{
			 // restringe aos arredores do centro
			 int dx = x - w / 2;
			 int dy = y - h / 2;
			 if (dx * dx + dy * dy <= 625) // até 25px de distância do centro
				pontos.Add(new Point(x, y)); // adiciona pixel à lista
			}
		}
	 }

	 if (pontos.Count == 0) // se nenhum ponto for válido
	 {
		loga("Centro não encontrado."); // loga erro
		return;
	 }

	 // calcula média dos pontos claros encontrados
	 int soma_x = 0;
	 int soma_y = 0;

	 foreach (Point p in pontos)
	 {
		soma_x += p.X;
		soma_y += p.Y;
	 }

	 int cx = soma_x / pontos.Count; // centro X estimado
	 int cy = soma_y / pontos.Count; // centro Y estimado

	 loga($"Centro estimado: {cx}, {cy}"); // loga resultado

	 // pode preencher em textbox se quiser:
	 // tb_debug1.Text = cx.ToString();
	 // tb_debug2.Text = cy.ToString();
	}

	private void button15_Click(object sender, EventArgs e)
	{
	 loc pranta = new loc(int.Parse(tb_debug1.Text), int.Parse(tb_debug2.Text)); // pega valores dos textboxes
	 andaplanta(pranta);

	}

	private void cb_BOM_CheckedChanged(object sender, EventArgs e)
	{

	}

	private void button16_Click(object sender, EventArgs e)
	{
	 Application.Exit(); // encerra o programa imediatamente
	}

	private void cb_pacifist_CheckedChanged(object sender, EventArgs e)
	{

	}

	private void button9_Click_1(object sender, EventArgs e)
	{
		on = false; // para o bot
	 dungeon = false; // desativa modo dungeon 
	}

	private void button9_Click_2(object sender, EventArgs e)
	{

	}
	// ----------------------------------------
	// BOTAO DEBUG - adiciona dados de avistamento 
	// e atualiza estimativa do mob com base nos últimos 5 pontos
	// ----------------------------------------
	private void button17_Click(object sender, EventArgs e)
	{
	 
	 checkme();
	 clog("mobs: " + me.mobs);





	}


	// ------------------------------------------
	// metodo giraok
	// ------------------------------------------
	// verifica se é possível fazer a curva até o ponto destino
	// sem parar de andar, com base na facing atual, posicao atual
	// raio de curva minimo pratico = 8.14
	// eixo Y precisa ser ajustado (comprimido)
	// considera que bot está sempre se movendo pra frente
	// retorna true se conseguir girar até o destino
	// false se for preciso parar pra virar
	bool podegirar(loc destino)
	{
	 const double raio = 8.14; // raio minimo de curva
	 const double yfix = 1.51; // fator de compressao do eixo Y
	 const double eps = 1e-6; // margem de erro flutuante

	 double dx = destino.x - me.pos.x; // diferença no X
	 double dy = (destino.y - me.pos.y) / yfix; // diferença no Y ajustada

	 double dist = Math.Sqrt(dx * dx + dy * dy); // distancia até o destino
	 if (dist < eps) return true; // se muito perto, considera que vira

	 double ang = (me.facing + 90) % 360; // converte facing do jogo para sistema trigonometrico
	 double rad = ang * Math.PI / 180.0; // transforma para radianos

	 // rotação reversa do sistema pro referencial do bot
	 double cos = Math.Cos(-rad);
	 double sin = Math.Sin(-rad);

	 double localx = dx * cos - dy * sin; // destino.x no sistema do bot
	 double localy = dx * sin + dy * cos; // destino.y no sistema do bot

	 if (localx < 0) return false; // está atrás, não dá pra virar andando
	 if (Math.Abs(localy) < eps) return true; // está reto, segue

	 // calcula raio necessário pra tangenciar o ponto
	 double r = (localx * localx + localy * localy) / (2 * localy);

	 return Math.Abs(r) >= raio; // compara com o raio minimo permitido
	}





	// ----------------------------------------
	// MÉTODO rendercursor
	// Desenha o cursor em um HBITMAP em memória (32x32)
	// Retorna o handle do HBITMAP para leitura posterior
	// ----------------------------------------
	public IntPtr rendercursor(IntPtr hCursor)
	{
	 IntPtr screenDC = GetDC(IntPtr.Zero);                     // pega o DC da tela
	 IntPtr memDC = CreateCompatibleDC(screenDC);              // cria DC em memória
	 IntPtr bmp = CreateCompatibleBitmap(screenDC, 32, 32);    // cria bitmap 32x32
	 SelectObject(memDC, bmp);                                 // vincula o bitmap ao DC
	 DrawIconEx(memDC, 0, 0, hCursor, 32, 32, 0, IntPtr.Zero, 0x0003); // desenha o cursor
	 ReleaseDC(IntPtr.Zero, screenDC);                         // libera DC da tela
	 DeleteDC(memDC);                                          // limpa o DC de memória
	 return bmp;                                               // retorna o handle do bitmap
	}



	private void button18_Click(object sender, EventArgs e)
	{
	 on = false;
	}

	// ----------------------------------------
	// BOTÃO RESET LOOT
	// Zera vetor lootfreq[25] e limpa a fila_fifo
	// Salva imediatamente no loot.txt
	// ----------------------------------------
	private void button19_Click(object sender, EventArgs e)
	{
	 for (int i = 0; i < 25; i++) lootfreq[i] = 0;     // zera todas as posições
	 fila_fifo.Clear();                                // limpa fila
	 total_loots = 0;                                  // zera contador

	 List<string> linhas = new List<string>();
	 linhas.Add("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"); // lootfreq zerada
	 linhas.Add("");                                                  // fila vazia
	 File.WriteAllLines("loot.txt", linhas);                          // salva imediatamente

	 loga("Loot resetado. Arquivo loot.txt zerado.");
	}

	private void button20_Click(object sender, EventArgs e)
	{
	 debugcura("FLASHHEAL", "HLIGHT nao curou suficiente, level >= 20, HP < 90"); // registra no debug
	}

	private void label25_Click(object sender, EventArgs e)
	{

	}

	// --------------------------------
	// MÉTODO MOVETO_CLAUDE V2 - NAVEGAÇÃO ORBITAL RECURSIVA
	// --------------------------------
	void moveto_claude(loc dest, loc next)
	{
	 loga($"Claude V2: dest=({dest.x},{dest.y}) next=({next.x},{next.y})");

	 checkme(); // atualiza posição atual

	 // ⭐ PROTEÇÃO ADICIONAL
	 if (dist(me.pos, dest) <= 1)
	 {
		loga("✅ Já está no destino!");
		return; // sai direto sem calcular ângulos
	 }

	 const double RAIO_CURVA = 6.84;
	 bool chegou_no_destino = false;

	 // --------------------------------
	 // LOOP PRINCIPAL RECURSIVO
	 // --------------------------------
	 while (!chegou_no_destino && on)
	 {
		checkme(); // atualiza posição a cada ciclo

		// --------------------------------
		// CLASSIFICAÇÃO DA CURVA
		// --------------------------------
		int yaw1 = getyaw(me.pos, dest);
		int yaw2 = getyaw(dest, next);
		int diferenca = Math.Abs(yaw1 - yaw2);
		if (diferenca > 180) diferenca = 360 - diferenca;
		int angulo_curva = 180 - diferenca;

		string tipo_curva;
		if (angulo_curva < 45) tipo_curva = "FECHADA";
		else if (angulo_curva < 135) tipo_curva = "MODERADA";
		else tipo_curva = "SUAVE";

		// --------------------------------
		// CALCULA DIREÇÃO DA CURVA
		// --------------------------------
		double dx1 = dest.x - me.pos.x;
		double dy1 = dest.y - me.pos.y;
		double dx2 = next.x - dest.x;
		double dy2 = next.y - dest.y;

		double produto_vet = dx1 * dy2 - dy1 * dx2;
		bool curva_esquerda = produto_vet > 0;

		// --------------------------------
		// DEFINE OBJETIVO BASEADO NO TIPO
		// --------------------------------
		loc objetivo;

		if (tipo_curva == "SUAVE")
		{
		 objetivo = dest; // vai direto para o centro
		}
		else // MODERADA ou FECHADA
		{
		 // calcula ponto de tangência
		 double offset = curva_esquerda ? RAIO_CURVA : -RAIO_CURVA;
		 double dist_atual_dest = Math.Sqrt(dx1 * dx1 + dy1 * dy1);

		 if (dist_atual_dest > 0) // evita divisão por zero
		 {
			double perp_x = -dy1 / dist_atual_dest * offset;
			double perp_y = dx1 / dist_atual_dest * offset;

			objetivo = new loc(
					(int)(dest.x + perp_x),
					(int)(dest.y + perp_y)
			);
		 }
		 else
		 {
			objetivo = dest; // fallback
		 }
		}

		loga($"Curva {tipo_curva} para {(curva_esquerda ? "ESQUERDA" : "DIREITA")}");
		loga($"Objetivo: ({objetivo.x},{objetivo.y})");

		// --------------------------------
		// NAVEGA ATÉ O OBJETIVO
		// --------------------------------
		press(ANDA); // inicia movimento

		while (dist(me.pos, objetivo) > 10 && on)
		{
		 checkme();
		 giralvo(objetivo); // mira no objetivo
		 wait(100);

		 // verifica se chegou no destino final
		 if (dist(me.pos, dest) <= 10)
		 {
			chegou_no_destino = true;
			break;
		 }
		}

		// --------------------------------
		// FASE ORBITAL (se não for curva suave)
		// --------------------------------
		if (!chegou_no_destino && tipo_curva != "SUAVE" && dist(me.pos, dest) < 200)
		{
		 loga("Iniciando FASE ORBITAL");

		 checkme(); // atualiza facing atual
		 int facing_atual = me.facing;
		 int yaw_para_next = getyaw(me.pos, next);
		 int delta_angular = facing_atual - yaw_para_next;

		 // normaliza para -180 a +180
		 if (delta_angular > 180) delta_angular -= 360;
		 if (delta_angular < -180) delta_angular += 360;

		 // calcula tempo com fator de drift
		 int tempo_base = (int)(Math.Abs(delta_angular) * 5.55);
		 int dist_ao_centro = dist(me.pos, dest);

		 if (dist_ao_centro < 200)
			tempo_base = (int)(tempo_base * 1.25); // aplica drift

		 loga($"Delta angular: {delta_angular}°, Tempo: {tempo_base}ms");

		 // executa o giro orbital
		 if (delta_angular > 0)
			aperta(AKEY, tempo_base); // gira esquerda
		 else if (delta_angular < 0)
			aperta(DKEY, tempo_base); // gira direita

		 loga("FASE ORBITAL concluída");
		}

		solta(ANDA); // para movimento

		// --------------------------------
		// VERIFICA SE CHEGOU NO DESTINO FINAL
		// --------------------------------
		checkme();
		if (dist(me.pos, dest) <= 10)
		{
		 chegou_no_destino = true;
		 loga("✅ CHEGOU NO DESTINO FINAL!");
		}
		else
		{
		 loga("🔄 RECALCULANDO próximo objetivo...");
		 wait(200); // pequena pausa antes do próximo ciclo
		}
	 }

	 solta(ANDA); // garantia final
	 loga("Claude V2 navegação concluída!");
	}
	// --------------------------------
	// MÉTODO ARRASTA_MOUSE_SUAVE
	// Movimento suave de arraste
	// --------------------------------
 void arrasta_mouse_suave()
	{
	 // --------------------------------
	 // PARÂMETROS DA TELA
	 // --------------------------------
	 int tela_w = Screen.PrimaryScreen.Bounds.Width;
	 int tela_h = Screen.PrimaryScreen.Bounds.Height;

	 int centro_x = tela_w / 2;
	 int centro_y = tela_h / 2;

	 // --------------------------------
	 // LÊ DESLOCAMENTO DA TEXTBOX
	 // --------------------------------
	 int pixels_total = int.Parse(tb_mouse_drag.Text);

	 // --------------------------------
	 // ARRASTE RÁPIDO EM STEPS
	 // --------------------------------
	 focawow();
	 mousemove(centro_x, centro_y);                    // posição inicial
	 mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);   // BOTÃO ESQUERDO

	 int steps = 10;                                   // MENOS steps = mais rápido
	 int step_size = pixels_total / steps;

	 for (int i = 1; i <= steps; i++)
	 {
		int nova_pos_x = centro_x + (step_size * i);
		mousemove(nova_pos_x, centro_y);
		wait(10);                                     // 10ms = BEM mais rápido!
	 }

	 mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);     // solta botão esquerdo

	 loga($"Arraste rápido: {pixels_total}px em {steps} steps");
	}

	// BOTAO 21
	private void button21_Click(object sender, EventArgs e)
	{
	 arrasta_mouse_suave(); // chama o método de arraste
	}


	// ----------------------------------------------
	// LOOP DE SCAN CONTÍNUO DO MAPA (COM COR DE TERRENO)
	// ----------------------------------------------
	async void loopscan()
	{
	 stopscan = false; // zera flag de parada

	 int? mx_ant = null, my_ant = null; // última posição conhecida

	 while (!stopscan)
	 {
		checkme(); // atualiza posição do player

		int mx = me.pos.x / 10; // reduz coordenada para índice de mapa
		int my = me.pos.y / 10;

		if (mx >= 0 && mx < w && my >= 0 && my < h) // dentro dos limites
		{
		 bool agua = me.swim; // lê flag vinda do pixel ou memória (depende da tua integração)

		 // ------------------------------------------
		 // TRAÇA LINHA DO PONTO ANTERIOR ATÉ O ATUAL
		 // ------------------------------------------
		 if (mx_ant != null && my_ant != null)
		 {
			int dx = mx - mx_ant.Value;
			int dy = my - my_ant.Value;
			int passos = Math.Max(Math.Abs(dx), Math.Abs(dy));

			if (passos > 0)
			{
			 for (int i = 0; i <= passos; i++)
			 {
				int x = mx_ant.Value + dx * i / passos;
				int y = my_ant.Value + dy * i / passos;

				if (x >= 0 && x < w && y >= 0 && y < h)
				{
				 if (agua)
				 {
					r[x, y] = 0;   // sem caminho
					g[x, y] = 0;   // sem direção
					b[x, y] = 255; // azul = água
				 }
				 else
				 {
					r[x, y] = 255; // caminho
					g[x, y] = 255; // força branco temporário
					b[x, y] = 255; // branco = caminho
				 }
				}
			 }
			}

			// define direção entre pontos
			int dx1 = mx - mx_ant.Value;
			int dy1 = my - my_ant.Value;
			byte bit = 0;

			if (dx1 == 0 && dy1 == -1) bit = 1;
			if (dx1 == 0 && dy1 == 1) bit = 2;
			if (dx1 == -1 && dy1 == 0) bit = 4;
			if (dx1 == 1 && dy1 == 0) bit = 8;
			if (dx1 == -1 && dy1 == -1) bit = 16;
			if (dx1 == 1 && dy1 == -1) bit = 32;
			if (dx1 == -1 && dy1 == 1) bit = 64;
			if (dx1 == 1 && dy1 == 1) bit = 128;

			if (!agua && bit != 0 && mx_ant >= 0 && my_ant >= 0 && mx_ant < w && my_ant < h)
			 g[mx_ant.Value, my_ant.Value] |= bit;

			byte rev = 0;
			if (bit == 1) rev = 2;
			else if (bit == 2) rev = 1;
			else if (bit == 4) rev = 8;
			else if (bit == 8) rev = 4;
			else if (bit == 16) rev = 128;
			else if (bit == 32) rev = 64;
			else if (bit == 64) rev = 32;
			else if (bit == 128) rev = 16;

			if (!agua && rev != 0)
			 g[mx, my] |= rev;
		 }

		 // atualiza última posição
		 mx_ant = mx;
		 my_ant = my;
		}

		await Task.Delay(500); // espera meio segundo
	 }

	 salvamapa(); // salva ao finalizar
	}


	// ----------------------------------------------
	// SALVAMAPA - SALVA MAPA NO DISCO
	// ----------------------------------------------
	// ----------------------------------------------
	// SALVAMAPA - SALVA O MAPA DIRETAMENTE NO ARQUIVO ORIGINAL
	// ----------------------------------------------
	void salvamapa()
	{
	 if (bmp != null) // se o bitmap original estiver carregado
	 {
		bmp.Dispose(); // libera o handle do arquivo
		bmp = null;    // evita uso posterior
	 }

	 Bitmap bmp2 = new Bitmap(w, h, PixelFormat.Format24bppRgb); // novo bitmap para salvar

	 Rectangle ret = new Rectangle(0, 0, w, h); // área da imagem
	 BitmapData data = bmp2.LockBits(ret, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb); // trava pra escrita
	 IntPtr ptr = data.Scan0; // ponteiro base do buffer
	 int stride = data.Stride; // bytes por linha (com padding)
	 byte[] raw = new byte[stride * h]; // buffer temporário

	 for (int y = 0; y < h; y++) // percorre linhas
		for (int x = 0; x < w; x++) // percorre colunas
		{
		 int i = y * stride + x * 3; // índice base do pixel
		 raw[i] = b[x, y];     // azul
		 raw[i + 1] = g[x, y];     // verde
		 raw[i + 2] = r[x, y];     // vermelho
		}

	 Marshal.Copy(raw, 0, ptr, raw.Length); // grava no bitmap
	 bmp2.UnlockBits(data); // destrava o bitmap

	 bmp2.Save(path); // salva no mesmo arquivo original
	 bmp2.Dispose();  // descarta bitmap temporário

	 MessageBox.Show("Mapa salvo em: " + path); // confirma
	}



	private void button1_Click_2(object sender, EventArgs e)
	{
	 stopscan = true;
	}

	// --------------------------------
	// BOTÃO FENCE A - DEFINE EXTREMIDADE A DA CERCA
	// --------------------------------
	private void bt_fenceA_Click(object sender, EventArgs e)
	{
	 checkme(); // atualiza posição atual

	 // Se clicar novamente em A, reseta todo o processo
	 if (a_definido || b_definido || p_definido)
	 {
		loga("⚠️ Processo de criação de cerca resetado!");
		a_definido = false;
		b_definido = false;
		p_definido = false;
	 }

	 // Armazena ponto A
	 temp_cerca_a = me.pos;
	 a_definido = true;

	 loga($"✅ Ponto A da cerca registrado em {temp_cerca_a.x},{temp_cerca_a.y}");

	 // Atualiza estado do botão save
	 bt_savefence.Enabled = (a_definido && b_definido && p_definido);
	}

	// --------------------------------
	// BOTÃO FENCE B - DEFINE EXTREMIDADE B DA CERCA
	// --------------------------------
	private void bt_fenceB_Click(object sender, EventArgs e)
	{
	 checkme(); // atualiza posição atual

	 // Se clicar novamente em B, reseta todo o processo
	 if (b_definido)
	 {
		loga("⚠️ Processo de criação de cerca resetado!");
		a_definido = false;
		b_definido = false;
		p_definido = false;
	 }

	 // Armazena ponto B
	 temp_cerca_b = me.pos;
	 b_definido = true;

	 loga($"✅ Ponto B da cerca registrado em {temp_cerca_b.x},{temp_cerca_b.y}");

	 // Atualiza estado do botão save
	 bt_savefence.Enabled = (a_definido && b_definido && p_definido);
	}

	// --------------------------------
	// BOTÃO FENCE P - DEFINE PONTO PROIBIDO
	// --------------------------------
	private void bt_fenceP_Click(object sender, EventArgs e)
	{
	 checkme(); // atualiza posição atual

	 // Se clicar novamente em P, reseta todo o processo
	 if (p_definido)
	 {
		loga("⚠️ Processo de criação de cerca resetado!");
		a_definido = false;
		b_definido = false;
		p_definido = false;
	 }

	 // Armazena ponto P
	 temp_cerca_p = me.pos;
	 p_definido = true;

	 loga($"✅ Ponto P (lado proibido) da cerca registrado em {temp_cerca_p.x},{temp_cerca_p.y}");

	 // Atualiza estado do botão save
	 bt_savefence.Enabled = (a_definido && b_definido && p_definido);
	}

	// --------------------------------
	// BOTÃO ADD FENCE - ADICIONA CERCA À LISTA (RENOMEADO)
	// --------------------------------
	private void bt_savefence_Click(object sender, EventArgs e)
	{
	 if (!a_definido || !b_definido || !p_definido)
	 {
		loga("❌ ERRO: Defina todos os 3 pontos (A, B e P) antes de adicionar!");
		return;
	 }

	 // Cria a cerca
	 Cerca nova_cerca = new Cerca(temp_cerca_a, temp_cerca_b, temp_cerca_p);

	 // Adiciona à lista de cercas
	 lcercas.Add(nova_cerca);

	 // Adiciona à listbox
	 lbwp.Items.Add(nova_cerca.ToPacked());

	 loga($"✅ CERCA ADICIONADA:");
	 loga($"   Ponto A: ({temp_cerca_a.x},{temp_cerca_a.y})");
	 loga($"   Ponto B: ({temp_cerca_b.x},{temp_cerca_b.y})");
	 loga($"   Ponto P: ({temp_cerca_p.x},{temp_cerca_p.y})");

	 // Reseta o processo
	 a_definido = false;
	 b_definido = false;
	 p_definido = false;
	 bt_savefence.Enabled = false;

	 loga("🔄 Pronto para criar nova cerca!");
	}

	private void label32_Click(object sender, EventArgs e)
	{

	}

	private void cb_COA_CheckedChanged(object sender, EventArgs e)
	{
 cb_COW.Checked = !cb_COA.Checked; // inverte checkbox COW se COA for marcado
	}

	private void cb_COW_CheckedChanged(object sender, EventArgs e)
	{
		cb_COA.Checked = !cb_COW.Checked; // inverte checkbox COA se COW for marcado
	}

	private void cb_scan_elite_CheckedChanged(object sender, EventArgs e)
	{

	}

	private void tb_timer_hours_TextChanged(object sender, EventArgs e)
	{

	}

	private void cb_nomurloc_CheckedChanged(object sender, EventArgs e)
	{
	 if (cb_nomurloc.Checked) MessageBox.Show("Atenção: Ataque a murlocs habilitado!");
	}




	// MULTI THREAD DE ATUALIZAÇÃO DAS STATS NO BACKGROUND 

	// --------------------------------
	// MÉTODO STARTBACKGROUNDUPDATES
	// Inicia thread para atualização contínua dos dados do drone a 10 FPS
	// --------------------------------
	private void StartBackgroundUpdates()
	{
	 if (threadRunning) return; // evita duplicar thread

	 threadRunning = true;
	 updateThread = new Thread(() =>
	 {
		while (threadRunning && on)
		{
		 try
		 {
			getstats(ref me);    // atualiza dados sem delays
			Thread.Sleep(100);   // 10 FPS sincronizado com addon
		 }
		 catch
		 {
			// proteção contra erros na thread
		 }
		}
	 });

	 updateThread.IsBackground = true; // morre com o programa
	 updateThread.Start();
	}

	// --------------------------------
	// MÉTODO STOPBACKGROUNDUPDATES  
	// Para a thread de atualização contínua de forma segura
	// --------------------------------
	private void StopBackgroundUpdates()
	{
	 threadRunning = false;              // sinaliza parada
	 updateThread?.Join(500);            // espera encerrar até 500ms
	}

	//------------------------
	/// INICIO METODOS CLAUDE 
	//-----------------------

	

	// --------------------------------
	// MÉTODO CALCULA_MEDIATRIZ  
	// Determina linha de simetria para curva suave
	// --------------------------------
	private (loc ponto_inicio_curva, double angulo_mediatriz) calcula_mediatriz(loc atual, loc dest, loc next)
	{
	 // --------------------------------
	 // VETORES DO TRIÂNGULO "BICO DE PATO"
	 // --------------------------------
	 double dx1 = dest.x - atual.x;     // vetor atual → dest
	 double dy1 = dest.y - atual.y;
	 double dx2 = next.x - dest.x;      // vetor dest → next  
	 double dy2 = next.y - dest.y;

	 // --------------------------------
	 // CORREÇÃO DO EIXO Y COMPRIMIDO
	 // --------------------------------
	 dy1 /= 1.515;  // aplica fator de correção
	 dy2 /= 1.515;

	 // --------------------------------
	 // CÁLCULO DOS ÂNGULOS E MEDIATRIZ
	 // --------------------------------
	 double angulo1 = Math.Atan2(dy1, dx1);        // direção atual → dest
	 double angulo2 = Math.Atan2(dy2, dx2);        // direção dest → next
	 double mediatriz = (angulo1 + angulo2) / 2;   // bissetriz

	 // --------------------------------
	 // PONTO DE INÍCIO DA CURVA
	 // --------------------------------
	 const double RAIO = 6.84;
	 double dist_antecipacao = RAIO * 1.5;         // margem de segurança

	 loc inicio_curva = new loc(
			 (int)(dest.x - Math.Cos(angulo1) * dist_antecipacao),
			 (int)(dest.y - Math.Sin(angulo1) * dist_antecipacao * 1.515) // reverte correção Y
	 );

	 // --------------------------------
	 // LOGS DETALHADOS PARA VALIDAÇÃO
	 // --------------------------------
	 loga("=== CÁLCULO MEDIATRIZ ===");
	 loga($"Atual: ({atual.x},{atual.y})");
	 loga($"Dest: ({dest.x},{dest.y})");
	 loga($"Next: ({next.x},{next.y})");
	 loga($"Vetor1: dx={dx1:F2}, dy={dy1:F2}");
	 loga($"Vetor2: dx={dx2:F2}, dy={dy2:F2}");
	 loga($"Ângulo1: {angulo1 * 180 / Math.PI:F1}°");
	 loga($"Ângulo2: {angulo2 * 180 / Math.PI:F1}°");
	 loga($"Mediatriz: {mediatriz * 180 / Math.PI:F1}°");
	 loga($"Início curva: ({inicio_curva.x},{inicio_curva.y})");
	 loga($"Distância antecipação: {dist_antecipacao:F2}");

	 return (inicio_curva, mediatriz);
	}








 }






}
