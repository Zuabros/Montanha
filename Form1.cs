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

	// VARIAVEIS DO DECAY
	DecaySession decay = new DecaySession();
	DecayTracker tracker = new DecayTracker();
	bool emCombate = false;


	// M20 - Variaveis de estado de jogo e personagem
	int res_y = Screen.PrimaryScreen.Bounds.Height;
	public bool on = true; // bot on
	public bool stopscan = false;
	int indexAtual = 0;


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
	public const int AUTOATTACK = UM;     // ataque automático
	public const int CLEARTGT = SETE;   // limpa o target
	public const int TARGETLAST = OITO;   // retarget último inimigo
	public const int HEARTHSTONE = N8;     // Hearthstone
	public const int HEALTHPOTION = N9;     // Poção de vida
	public const int MANAPOTION = N0;     // Poção de mana
	public const int ANDA = WKEY;   // comando de andar
	public const int SLOW = F2;     // debuff de lentidão (genérico)
	public const int ASSIST = F6;     // debuff de lentidão (genérico)
	public const int GCD = 1501;  // Global Cooldown (tempo de recarga global em milissegundos)
	public const int FOOD = F12;     // ataque automático

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
	public const int STONEFORM = NOVE;   // racial dos anões (caso use anão paladino)

	public const int DEVAURA = F3;     // Devotion Aura
	public const int RETAURA = F4;     // Retribution Aura
	public const int BOP = F7;     // Retribution Aura
	public const int CONAURA = F8;     // Concentration Aura
	public const int DSHIELD = F9;     // Divine Shield (Bubble)
	public const int FLASHHEAL = F10;     // Divine Shield (Bubble)

	// --------------------------------------------
	// SKILLS EXCLUSIVAS DO ROGUE
	// --------------------------------------------
	public const int SS = N1; // Sinister Strike
	public const int EVIS = N2; // Eviscerate
	public const int THROW = N3; // Throwing knife
	public const int STEALTH = N4; // Stealth
	public const int PICKPOCKET = N5; // Pickpocket
	public const int EVASION = N6; // Evasion
	public const int SAD = N7; // Evasion
	public const int EXPOSE_ARMOR = N0; // EXPOSE ARMOR

	public const int GOUGE = F6; // Gouge
	public const int KICK = F7; // Kick
	public const int VANISH = F8; // Vanish
	public const int SPRINT = F9; // Sprint
	public const int KS = 1; // Kidney Shot
	public const int CS = 1; // Cheap Shot

	// --------------------------------------------
	// TIPOS DE CRIATURAS
	// --------------------------------------------
	public const int HUMANOID = 50; // humanoide
	public const int BEAST = 100; // besta
	public const int PLAYER_MELEE = 105; // player mané ogro (não-caster)
	public const int PLAYER_CASTER = 110; // player mané caster
	public const int UNDEAD = 150; // morto-vivo
	public const int DEMON = 200; // demônio
	public const int ELEMENTAL = 210; // elemental
	public const int MECHANICAL = 220; // mecânico
	public const int DRAGONKIN = 230; // dragonete
	public const int GIANT = 240; // gigante
	public const int CRITTER = 80; // criatura pequena (critter)

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
	 carrega_loot(); // carrega frequencia de pontos de loot 
	 cb_nohumanoid.Checked = true; // ativa filtro de humanoides por padrão
																 


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
	 // B: vazio


	 // ----------------------------------------
	 // PIXEL 1 – INVENTÁRIO, ERROS E RECURSO
	 // ----------------------------------------
	 // R: Slots livres nas bags (255 - slots livres)
	 // G: Erros:
	 //     128 = de costas (Facing wrong way)
	 //      64 = fora de alcance (Out of range)
	 //      0  = normal
	 // B: Recurso primário (mana, rage, energia etc.) proporcional (0–255)


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
	 // PIXEL 9 – REAÇÃO E AMEAÇA DO TARGET
	 // ----------------------------------------
	 // R: 255 se hostil
	 // G: 255 se amigável
	 // R+G = 255/255 se neutro
	 // B: 255 se com ameaça máxima (aggro fixado)

	 // ----------------------------------------
	 // PIXEL 10 (Paladino) – Judgement e Auras
	 // ----------------------------------------
	 // R: 255 se Judgement em alcance
	 // G: Cooldown restante de Judgement (0–255 proporcional)
	 // B: Bitflags de auras:
	 //     2 = Crusader | 4 = Devotion | 8/16/32 = Resistências
	 //     64 = Concentration | 128 = Retribution


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
	 // PIXEL 10 (Rogue) – Combo Points + Throw + Stealth
	 // ----------------------------------------
	 // R: bits 0–2 = número de combo points (0 a 7)
	 //     bit 0 = soma 1
	 //     bit 1 = soma 2
	 //     bit 2 = soma 4
	 //     bit 3 = throw_up (faca pronta para uso e em range)
	 //     bit 4 = stealth_up (é possível ativar Stealth agora)
	 //     bit 5 (32) evasion up
	 //     bit 6 (64) slice and dice up
	 // 	  bit 7 (128) expose armor pronto + energy 
	 // G: bit 0 = has_stealth (está com aura de Stealth ativa)
	 //		 bit 1 = has_SAD (slice and dice ativo)
	 // B: 1 = target tem expose armor debuff 

	 // ----------------------------------------
	 // PIXEL 11 a 15 (Rogue) – Vazios por enquanto
	 // ----------------------------------------
	 // R, G, B = 0




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
	 if (false && cb_debug.Checked && cb_log.Checked)
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
	 if (false && cb_debug.Checked && cb_log.Checked)
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
if (false && cb_debug.Checked && cb_log.Checked) // codigo desabilitado
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
 if (false && cb_debug.Checked && cb_log.Checked) // codigo desabilitado pra nao sujar interface
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

 if (false && cb_debug.Checked && cb_log.Checked)  // se debug e loga estiverem marcados
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
		 aperta(SKEY, 1200); // backpedal 1.2s
		}
		loga("Mobs: " + mobs);
	 }
	 me.mobs = mobs;

	 // debug visual
	 tb_hp.Text = v_hp.ToString();



	 // ------------------------------------------
	 // Pixel 1 - Mana (canal B) / Bags (canal R) / Erros de combate (canal G)
	 // ------------------------------------------

	 // MANA
	 int v_mana = (pixels[1].b * 100) / 255;            // canal B (0–255) convertido em porcentagem
	 e.mana = v_mana;                                   // atualiza atributo de mana
	 tb_mana.Text = v_mana.ToString();                  // exibe no textbox (debug)

	 // SLOTS LIVRES NAS BAGS
	 int v_slots = 255 - pixels[1].r;                   // canal R invertido → slots livres reais
	 e.freeslots = v_slots;                             // atualiza slots livres

	 // ERROS DE COMBATE (bitmask no canal G)
	 int g_erro = pixels[1].g;                           // canal G codifica erros combinados
	 e.wrongway = (g_erro & 128) > 0;                   // 128 = "You are facing the wrong way!"
	 e.outofrange = (g_erro & 64) > 0;                  //  64 = "Out of range" ou "You are too far away!"


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
		if (cb_debug.Checked) tb_yaw.Text = e.facing.ToString();

		// velocidade
		e.spd = 255 - pixels[4].g; // canal G invertido
		if (cb_debug.Checked) tb_spd.Text = e.spd.ToString();

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
		tar.ieslite = (pixels[7].g & 32) > 0;        // G: bit 5 → elite

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


	 // -------------------------------------
	 // NOVO PIXEL 9: REAÇÃO E AMEAÇA DO TARGET (Movido do antigo Pixel 15)
	 // -------------------------------------
	 if (pixels.Count > 9)
	 {
		// mood / reação do target
		bool hostile = pixels[9].r > 200 && pixels[9].g < 50;  // vermelho puro
		bool neutral = pixels[9].r > 200 && pixels[9].g > 200; // amarelo
		bool friendly = pixels[9].r < 50 && pixels[9].g > 200; // verde

		if (hostile) tar.mood = -1; // hostil 
		else if (friendly) tar.mood = 1;
		else tar.mood = 0; // neutral

		tb_mood.Text = (tar.mood == -1) ? "Hostile" :
																 (tar.mood == 1) ? "Friendly" : "Neutral";

		// canal azul = 255 se o mob está me atacando (aggro confirmado)
		tar.aggroed = (pixels[9].b > 250); // reuse da propriedade para "aggro ativo"
	 }

	 // ====================================================================
	 // PIXELS ESPECÍFICOS DE ROGUE (10 a 15)
	 // ====================================================================
	 if (e.classe == ROGUE) // se for Rogue, lê o Pixel 10
	 {
		// -------------------------------------
		// PIXEL 10 – COMBO POINTS + CDs + STEALTH
		// R: bits 0–2 = combo points (0 a 7)
		// R: bit 3 (valor 8)   = throw_up
		// R: bit 4 (valor 16)  = stealth_up (pode usar Stealth)
		// R: bit 5 (valor 32)  = evasion_up (pode usar Evasion)
		// R: bit 6 (valor 64)  = SAD_up (Slice and Dice pode ser usado)
		// R: bit 7 (valor 128) = expose_armor_up (pode usar Expose Armor)
		// G: bit 0 (valor 1)   = stealth (está em Stealth)
		// G: bit 1 (valor 2)   = has_SAD (aura Slice and Dice ativa)
		// B: bit 0 (valor 1)   = target com debuff Expose Armor
		// -------------------------------------
		int r10 = pixels[10].r;
		int g10 = pixels[10].g;
		int b10 = pixels[10].b;

		rog.combo = r10 & 7;           // bits 0–2 = combo points
		rog.throw_up = (r10 & 8) != 0;    // bit 3 = throw_up
		rog.stealth_up = (r10 & 16) != 0;   // bit 4 = stealth_up
		rog.evasion_up = (r10 & 32) != 0;   // bit 5 = evasion_up
		rog.SAD_up = (r10 & 64) != 0;   // bit 6 = Slice and Dice pode ser usado
		rog.expose_armor_up = (r10 & 128) != 0;  // bit 7 = Expose Armor pode ser usado

		rog.stealth = (g10 & 1) != 0;    // bit 0 = aura Stealth ativa
		rog.has_SAD = (g10 & 2) != 0;    // bit 1 = aura Slice and Dice ativa

		rog.has_expose_armor = (b10 & 1) != 0;    // bit 0 = target com debuff Expose Armor

		tb_combos.Text = rog.combo.ToString(); // exibe combo points

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
public void aperta(byte key, int time = 50) // time 0 = pressiona, time 2 = solta
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



	// --------------------------------
	// MÉTODO GIRALVO 5.2 - GIRA O PERSONAGEM PARA A COORDENADA ALVO (versão 0–360)
	// --------------------------------
	public void giralvo(loc alvo)
	{
	 giraface(getyaw(me.pos, alvo), dist(me.pos, alvo));
	}


	// M11 - MÉTODO DIST - CALCULA A DISTÂNCIA ENTRE DUAS COORDENADAS (LOC).
	public int dist(loc orig, loc tar)
{
double distance = Math.Sqrt(Math.Pow(Math.Abs(orig.x - tar.x), 2) +
														Math.Pow(Math.Abs(orig.y - tar.y), 2));
return (int)distance;
}








// MÓDULO 18 - VARIÁVEL GLOBAL - OBJETO PLAYER e TARGET
public element me;
public element tar;
public palatable pala = new palatable(); // inicializa tabela de status de paladino 
public roguetable rog = new roguetable(); // inicializa tabela de status de rogue
	public HashSet<loc> hash_planta = new HashSet<loc>(); // inicializa tabela de plantas encontradas

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
	 // VIRA POUCO E MARCADO ANDA 
	 if (cb_anda.Checked && Math.Abs(delta(me.pos, destino)) < 60)            
		press(WKEY); // ANDA 
	 else
		solta(WKEY); // PARA DE ANDAR 
	 long last = 0; // marca o tempo do último evento em milissegundos
	 ///------------------------------------------------------
	 //                 COMEÇA A ANDAR  
	 //--------------------------------------------------------
	 do
	 {
		// --------------------------------
		// LOOP DE PAUSA
		// --------------------------------
		atualizamapa(me.pos);
		get_minimap(); // atualiza minimapa

		nao_afoga(); // se afogando, tenta nadar

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
		 aperta(WKEY, 2); // solta W
		 if (cb_log.Checked) loga("Timeout no moveto() — passando para o próximo ponto.");
		 return;
		}
		temp++;
		checkme(); // atualiza estado do personagem
	// -----------------------------
	// BUFFS OUT OF COMBAT 
	// -----------------------------
		// PALADINO
		if (me.classe==PALADIN) bless(me); // 
		// ROGUE 
		// ainda não implementado, mas pode ser feito aqui
//------------------------		
		// -----------------------------
		// ANDAR (direção e pulo)
		// -----------------------------
		if (cb_anda.Checked)
		{
		 if (timeout.ElapsedMilliseconds - last >= 4000) // passaram 4 segundos?
		 {
			aperta(SPACEBAR); // dá pulinho a cada 4 segudnos
			last = timeout.ElapsedMilliseconds; // atualiza o tempo do último disparo
		 }
		 checkme();
		 drawmap(destino);
		 loga($"Distancia do waypoint: {dist(me.pos, destino)}");
		 giralvo(destino);
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
		 if (!me.hastarget) aperta(TAB);

		 if (cb_anda.Checked && !me.combat && dist(me.pos, oldloc) <= 10)
		 {
			stuckcount++;
			//if (cb_log.Checked) loga("enrosco lvl " + stuckcount);
			if (stuckcount > 0)
			{
			 unstuck();       // executa pulo + giro se necessário
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
		// LIMPA TARGET MORTO FORA DE COMBATE
		// -----------------------------
		if (!me.combat && tar.morreu && me.hastarget)
		{
		 aperta(SETE); // limpa target se estiver morto
		 if (cb_log.Checked) loga("Target morto fora de combate — limpando.");
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
		 solta(ANDA); // para de andar
		 if (cb_log.Checked) loga("Entrou em combate!");
		 combatloop(); // entra na rotina de combate


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

		 if (cb_apagacinza.Checked) clica(new loc(5, 5), 1); // APAGA UM ITEM CINZA DA BAG, SE TIVER (botão criado pelo addon de apagar itens cinzas)
		 if (has(me.freeslots) || cb_loot_cloth.Checked) // só loota se estiver ativado, e se tiver espaço ou for loot de pano

		 {
			if (!me.combat)
			{
			 if (cb_loot.Checked)
			 {
				int loopcount = 0;

				while (true)
				{
				 checkme();                  // atualiza dados
				 if (me.combat) break;      // se entrou em combate, para tudo

				 if (loopcount++ >= 6)      // tentou 6 vezes, desiste
				 {
					loga("Loop de loot interrompido após 6 tentativas.");
					break;
				 }

				 loc p = scanloot();        // tenta encontrar algo clicável
				 if (p.x < 0) break;        // nada encontrado

				 clica(p);                  // clica diretamente — cursor já é validado no scanloot
//				 wait(100);
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
		 else if (me.armabroken) // ARMA QUEBRADA – MAIN OU OFFHAND
		 {
			loga("Arma quebrada detectada. Usando Hearthstone.");
			HS(); // retorna pra reparar
		 }

		 //if (cb_anda.Checked) aperta(WKEY, 0); // retoma andar se permitido
		 return; // sai do moveto após combate.

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
		// RECUPERAÇAO E PREPARO PRÉ COMBATE - ROGUE
		//----------------------------------
		else if (me.classe == ROGUE)
		{
		 // espera recuperar energia
		 if (me.hp < 80)
		 {
			para(); // para de andar se estiver andando
			loga($"Esperando recuperação de HP: {me.hp}");
			aperta(F12); // COMIDA 
			while (me.hp < atoi(tb_rogue_eat_at)  && !me.combat)
			{
			 wait(1000);
			 checkme();
			}
		 }
		}
		//--------------------------------------
		// CONTINUA ANDANDO ATÉ CHEGAR NO LOCAL. RESTART DO LOOP

	 } while (on && dist(me.pos, destino) > (catando_planta ? 70 : 120)); // enquanto ativo e longe (20 se catando)

	 loga("Alvo atingido. Partindo para proximo alvo.");
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
		// REGIÃO 1: MANA < 20% → ENTRA EM MODO SEM MANA
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

	 aperta(TAB);       // seleciona o alvo mais próximo
	 checkme();         // atualiza status do alvo

	 string tipo = "";  // tipo textual do alvo

	 if (tar.type == 50 && cb_humanoid_patrol.Checked)
		tipo = "humanoide";
	 else if (tar.type == 240 && cb_giant_patrol.Checked)
		tipo = "gigante";
	 else if (tar.israre && cb_rare_patrol.Checked)
		tipo = "raro";
	 else if (tar.ieslite && cb_elite_patrol.Checked)
		tipo = "elite";

	 if (me.hastarget && tipo != "")
	 {
		int seg = atoi(tb_wait_patrol); // segundos a esperar
		loga($"Detectado patrulha do tipo {tipo}. Esperando {seg} segundos.");
		if (me.classe == ROGUE && !rog.stealth)
		 aperta(STEALTH); // entra em stealth se for rogue e não estiver stealth
		espera(seg); // aguarda o tempo configurado
	 }
	 else
		loga("Nenhum elite ou patrulha detectado.");
	}



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
	 scan_elites(); // verifica se tem elite no target e ajusta o pull se necessário


	 // --------------------------------------------
	 // VERIFICAÇÃO DO TARGET - ESCOLHA ENTRE DOIS
	 // --------------------------------------------

	 bool bomtarget()
	 {
		// avalia se o target atual é considerado bom
		if (!me.hastarget) return false;
		if (cb_pacifist.Checked) return false;
		if (tar.hp == 0 || tar.morreu) return false;
		if (isgray(me.level, tar.level) && !cb_killgray.Checked) return false;
		if (tar.mood == 1) return false; // amarelo = não hostil
		if (tar.combat) return false;
		if (tar.ieslite && cb_noelite.Checked) return false; // não é elite se não estiver marcado
		if (tar.hp < 100) return false; // já apanhou
		if (tar.level > me.level + Convert.ToInt32(tb_pullcap.Text)) return false;
		if ((tar.type == 50 && cb_nohumanoid.Checked) || (tar.type == 230 && cb_nodragonkin.Checked)) return false;
		
		return true;
	 }

	 // pega primeiro target
	 if (!me.hastarget) aperta(TAB,400);
	 checkme(); // atualiza status do segundo target
	 int lvl1 = tar.level;
	 int mood1 = tar.mood;
	 bool t1_ok = bomtarget();

	 // pega segundo target
	 aperta(TAB,400);
	 checkme(); // atualiza status do segundo target
	 int lvl2 = tar.level;
	 int mood2 = tar.mood;
	 bool t2_ok = bomtarget();

	 if (!t1_ok && !t2_ok)
	 {
		loga("Nenhum target válido encontrado. Limpando o alvo.");
		aperta(CLEARTGT);
		return;
	 }
	 else if (t1_ok && !t2_ok)
	 {
		loga("Apenas o primeiro target é válido. Retornando para ele.");
		aperta(IGUAL);
	 }
	 else if (!t1_ok && t2_ok)
	 {
		loga("Apenas o segundo target é válido. Mantendo ele.");
	 }
	 else // ambos válidos
	 {
		if (mood1 != mood2)
		 if (mood1 == -1 && mood2 == 0)
		 {
			loga("Primeiro target é hostil e o segundo é neutro. Preferindo o hostil.");
			aperta(IGUAL);
		 }
		 else
			loga("Segundo target é hostil ou ambos são neutros. Mantendo o segundo.");
		else if (mood1 == -1) // ambos hostis
		 if (lvl1 > lvl2)
		 {
			loga("Ambos hostis. Primeiro tem nível mais alto. Retornando para ele.");
			aperta(IGUAL);
		 }
		 else
			loga("Ambos hostis. Segundo tem nível igual ou mais alto. Mantendo o segundo.");
		else
		 loga("Ambos neutros. Mantendo o segundo por padrão.");
	 }


	 // --------------------------------------------
	 // CORRE ATE O MOB E ATACA (PULL)
	 // --------------------------------------------
	 int ticker = 0; // contador de ciclos
	 if (me.hastarget && tar.hp == 100 && !me.combat) // alvo válido e fora de combate
	 {
		loga("Alvo válido encontrado. Iniciando pull.");
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
			 aperta(JUDGEMENT);
			 aperta(AUTOATTACK);
			}
			else
			{
			 aperta(AUTOATTACK); // ataca direto se nenhuma opção acima for válida
			 aperta(INTERACT); // anda até o mob11
			}

			if (ticker++ % 8 == 0)
			 aperta(PULA); // pulo de simulação humana a cada 2s

			aperta(INTERACT); // anda até o mob11

			wait(200);   // aguarda meio segundo
			checkme();   // atualiza status
		 }
		 // ---------------------INICIO PULL ROGUE ----------------
		 else if (me.classe == ROGUE) // se for rogue
		 {
			aperta(AUTOATTACK);   // ativa autoattack
			aperta(INTERACT);     // começa a andar até o mob
			checkme(); // atualiza status

			// novo: se faca estiver pronta, lança e para de andar
			if (rog.throw_up) // chegou em range da faca
			{
			 // ---------------------------------------------
			 // PICKPOCKET / PULL STEALTH AO INVÉS DE FACA 
			 // ---------------------------------------------
			 if ((tar.type == HUMANOID && cb_pickpocket.Checked) || (cb_stealth_pull.Checked && rog.stealth_up))
			 {
				loga("Humanoide – tentando Pull em Stealth com Pickpocket.");
				aperta(STEALTH); // entra em stealth
				int t0 = Environment.TickCount;
				int lastInteract = Environment.TickCount;

				while (true) // loop com timeout real
				{
				 if (Environment.TickCount - lastInteract >= 500)
				 {
					aperta(INTERACT); // ajusta curso até o mob
					lastInteract = Environment.TickCount;
				 }

				 if (me.melee && cb_pickpocket.Checked) aperta(PICKPOCKET); // tenta pickpocket
				 if (me.melee) aperta(SS); // sempre tenta abrir com sinister strike

				 checkme();

				 if (me.combat) return; // combate iniciado

				 if (tar.hp<100 || !me.hastarget || Environment.TickCount - t0 > 15000)
				 {
					loga("Timeout de segurança: Pull abortado.");
					aperta(STEALTH);
					para();
					aperta(CLEARTGT);
					return;
				 }
				}
				
			 }

			 // ---------------------------------------------
			 // PULL COM FACA
			 // ---------------------------------------------
			 loga("Parando para lançar faca.");
			 para(); // para de andar

			 if (!rog.throw_up) // perdeu o range antes de lançar
			 {
				loga("Muito perto da faca. Avaliando Stealth.");
				if (rog.stealth_up && !rog.stealth)
				{
				 loga("Ativando Stealth após perder range da faca.");
				 aperta(STEALTH);
				 // espera(1);
				}
				aperta(INTERACT); // segue andando stealth ou normal
			 }
			 else
			 {
				casta(THROW); // lança faca (Throw)
				for (int i = 0; i < 15; i++) // 10 ciclos de 500ms = 5 segundos
				{
				 wait(300);      // espera 0.5s
				 checkme();      // atualiza status
				 if (me.melee || me.mobs > 0)  // se entrou em combate, sai do loop
					break;
				}
				if (me.combat) return;
			 }
			}

			// ---------------------------------------------
			// PULL COM SINISTER STRIKE (range direto)
			// ---------------------------------------------
			if (rog.ss_up) casta(SS);

			checkme();
			if (me.combat) return;

			if (ticker++ % 16 == 0) aperta(PULA); // pulo humano a cada 2s
			aperta(INTERACT); // continua andando até o mob
		 }
		 // ---------------------FIM PULL ROGUE---------------------



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
		aperta(SKEY); // pequeno toque para trás
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
	// MÉTODO COMBATLOOP - ROTINAS DE COMBATE
	// --------------------------------
	// Variáveis de controle
	long last_purify = 0; // armazena o último uso do purify em ms
	int last_kill_time = Environment.TickCount; // tempo do último combate vencido
																							// --------------------------------

	public void combatloop()
	{
	 int combat_ticker = 0; // contador de ciclos de combate
	 solta(ANDA); // PARA DE ANDAR
	 aperta(SKEY); // anda para trás se necessário
	 bool ja_deu_backpedal = false; // se estiver apanhando muito anda um pouco para tras pra nao dar as costas 
	 bool jalogou = false; // se já logou o decay
	 if (!emCombate) // decay
	 {
		decay.Start(me.hp);
		emCombate = true;
	 }
	 int ticker= 0; // contador de ciclos
	 do
	 {
		combat_ticker++; // incrementa contador de combate 
		ticker++;
		Func<bool> has_seal = () => pala.sor || pala.soc || pala.sow || pala.sol || pala.sotc; // verifica se tem algum seal ativo
		Func<int, bool> mana = (p) => me.mana > p;

		if (me.combat) // calculo do decay
		{
		 decay.Update(me.hp);
		 int curdecay = decay.Current(tracker.Average); // exibe o decay atual em hp/min (sliding window); nos primeiros 10s usa média dos combates anteriores
		 if (curdecay <  2000)  tbdecay.Text = curdecay.ToString(); // exibe o decay atual no textbox 
		}


		// --------------------------------
		// ROTINAS ALL-CLASS
		// --------------------------------
		// NAO DEIXA AFOGAR 
		//------------------------------
		if (!dungeon) nao_afoga(); // nada para cima se estiver afogando; permite nadar em dungeons

		//----- HEALTH POTION --------
		if (me.hp < 30 && me.mana < 40) // Tá no bico do corvo 
		 aperta(HEALTHPOTION);
		else if (me.hp > 80 && me.mana < 15) // lay on hands ou drain mana
		 aperta(MANAPOTION);
		//-----------------------------------
		// ASSIST NO TANK EM DUNGEON 
		//-----------------------------------
		if (dungeon && cb_assist_tank.Checked && !me.hastarget || tar.morreu)
		{
		 aperta(F6); // limpa o target e assiste o tank
		}
		// -----------------------------------------
		// recuo tático se estiver apanhando demais
		// -----------------------------------------
		
		if (me.mobs>1 && (me.dazed || (!dungeon && !ja_deu_backpedal && me.hp < 85))) // se estiver apanhando de mais ou com 2 mobs batendo
		{
		 
		 int limiar = int.Parse(tb_back_limiar.Text); // Lê o valor do limiar 
		 int decay = int.Parse(tbdecay.Text);// Obtém os valores de decay e avg_decay 
		 int avg_decay = int.Parse(tbavdecay.Text);
		 if (decay > 2000) decay = int.Parse(tbavdecay.Text); // se o decay estiver irreal
																													// Executa a lógica apenas se o mob não estiver castando e o decay atual for maior que o limiar

		 bool precisa_backpedal = me.dazed || (tar.type != DRAGONKIN && !tar.casting && decay > limiar && tar.hp > 20);
// Fica dazed ou toma muito dano se sem segundo mob batendo nas costas. Então dá um passinho para trás. 

		 if (precisa_backpedal)
		 {
			if (!me.dazed) ja_deu_backpedal = true; // só uma vez por combate
			if (me.dazed) loga("Dazed! Backpedal para evitar mob batendo atrás."); // loga se estiver atordoado
			else loga($"Dando Backpedal: decay = {int.Parse(tbdecay.Text)}");
			if (!me.dazed) aperta(SKEY, 1200);     // anda pra trás mantendo o facing
			if (me.dazed) aperta(SKEY, 1200);     // anda pra trás mantendo o facing
			aperta(AUTOATTACK);
			
		 }
		 else if (!jalogou)
		 {
			loga($"Backpedal não necessário: decay = {int.Parse(tbdecay.Text)}");
			jalogou = true; // loga o decay apenas uma vez por combate
		 }
		}
		// -----------------------------------------
		// GIRA PARA O ALVO SE ESTIVER APANHANDO DE COSTAS
		// -----------------------------------------
		if (!dungeon && me.wrongway && cb_wrong_gira.Checked)
		{
		 roda(25); // gira pra manter face no inimigo
		 loga("De costas para o alvo! Ativando correção.");
		 wait(2000); // espera 2s para não ficar girando muito
		 checkme(); // atualiza status após girar
		}

		// --------------------------------
		// VERIFICA SE COMBATE TERMINOU
		// --------------------------------
		wait(100);
		checkme();
		if (!dungeon && !tar.aggroed) // target morto ou aparentemente inválido
		{

		 if (!tar.aggroed) // confirma que ainda está inválido
		 {
			aperta(CLEARTGT); // limpa o target
			checkme(); // atualiza estado após limpar

			if (!me.combat) break; // combate terminou, sai do loop
			continue; // ainda em combate, reinicia ciclo
		 }
		}
		else // Mob vivo e com agro em mim

		{
		 aperta(INTERACT);               // anda até o alvo se estiver longe
		 if (ticker%6==0) aperta(PULA); // da uns pulinhos a cada 3 ciclos pra pular a cerca ao ir atras do target 
		}

		//---------------------ROTINA EXCLUSIVA PALADINO ---------------------
		if (me.classe == PALADIN)           //  
		{
		 wait_cast();                    // espera fim de cast se tiver algum

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
			aperta(DEVAURA,1500); // ativa Devotion para defesa

		 // hp > 50% e quer Retribution Aura, mas não está ativa
		 else if (!dungeon && me.hp > 50 && cb_retaura.Checked && !pala.retribution)
			aperta(RETAURA,1500); // ativa Retribution para dano
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
			aperta(PULA);                                           // pula antes de atacar (visual agressivo)
			aperta(JUDGEMENT);                                      // executa o Judgement
		 }

		 if (should_stun())                                          // verifica se deve usar stun
			aperta(HOJ);                                            // aplica Hammer of Justice

		 solta(ANDA);                                            // para de andar no final do turno
		}
		// -----------------------------------------------
		// ROTINA DE COMBATE ROGUE
		// -----------------------------------------------
		else if (me.classe == ROGUE)
		{
		 // ------------------------------------------
		 // AUTOATTACK + PULOS
		 // ------------------------------------------
		 if (tar.mood != 1 && !me.autoattack) aperta(AUTOATTACK); // garante autoattack se mob hostil
		 if (ticker % 6 == 0) aperta(PULA);                        // human-like pulo eventual

		 // ------------------------------------------
		 // DEFESA: EVASION se vida baixa
		 // ------------------------------------------
		 if (me.hp < atoi(tb_evasion))
		 {
			if (rog.evasion_up) aperta(EVASION);                 // ativa Evasion se disponível e HP < limiar
		 }
		 if (me.hp < 35 && me.hp_potion_rdy)
			aperta(HEALTHPOTION); // usa poção de cura se HP < 35% e poção pronta

		 // ------------------------------------------
		 // MOVIMENTO: aproximação se fora de melee
		 // ------------------------------------------
		 if (!me.melee) aperta(INTERACT);                         // tenta chegar no alvo via Interact With

		 // ------------------------------------------
		 // COMBATE SEM COMBO POINTS
		 // ------------------------------------------
		 if (rog.combo == 0)
		 {
			if (rog.ss_up) casta(SS);  // sem combo → gerar com SS
		 }
		 // ------------------------------------------
		 // COMBATE COM COMBO POINTS
		 // ------------------------------------------
		 else
		 {
			bool finalizavel = tar.hp <= 25;
			bool rotina = rog.combo >= atoi(tb_evis_cp);  // combo ideal
			bool pode_evis = rog.evis_up && (finalizavel || rotina);

			if (pode_evis)
			{
			 casta(EVIS);  // mob vai morrer ou combo cheio
			}
			else if (!rog.has_SAD && rog.SAD_up)
			{
			 casta(SAD);   // não tem Slice and Dice → aplica
			}
			else if (rog.has_SAD && rog.expose_armor_up && !rog.has_expose_armor && tar.hp > 70)
			{
			 loga("Aplicando Expose Armor.");
			 casta(EXPOSE_ARMOR);
			 espera(1);
			 checkme();
			}
			else if (rog.ss_up && mana(45))
			{
			 casta(SS);  // fallback → gerar mais combo
			}
		 
		}
		}
		checkme();

	 } while (me.combat); // FIM DO LOOP DE COMBATE 

	 //---------------------------------------------
	 // TERMINA O COMBATE 
	 // ---------------------------------------------
	 loga($"Combate encerrado. Ciclos: {combat_ticker}");
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
	 //--------------------------------
	 }
	// --------------------------------
	// MÉTODO DE ESPERA VIGILANTE
	// --------------------------------
	void espera(int seconds=1)
	{
	 solta(ANDA); // para de andar antes de esperar
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
		 casta(HLIGHT);            // casta HLIGHT
		 wait_cast();              // espera o cast
		 checkme();                // atualiza status após cast

		 debugcura("HLIGHT", motivo); // registra nos debug e caixas

		 if (me.level >= 20 && me.hp < 90)
		 {
			casta(FLASHHEAL);     // usa FLASHHEAL se necessário
			debugcura("FLASHHEAL", "HLIGHT nao curou suficiente, level >= 20, HP < 90"); // registra no debug
		 }
		 else if (me.mobs <2 && me.level < 20 && me.hp < 60)
		 {
			casta(HLIGHT);            // casta mais um HLIGHT
			debugcura("OUTRO HLIGHT", "O primeiro nao curou suficiente, level < 20"); // registra no debug
			wait_cast();              // espera o cast
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
		 aperta(HLIGHT, 1000);        // casta HLIGHT durante o stun
		 wait_cast();
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
		aperta(HLIGHT, 1000);           // casta holy light
		wait_cast();                    // espera o cast
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
	 tb_debug4.Text = dist(me.pos, alvo).ToString(); // distancia entre mim e o alvo
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

	 moveto(go);
		
	 
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
	// (equivalente a lb_yaw.Text do botão drawmap, ou seja, 360 - yaw visual)
	// -----------------------------------------------------------
	int getyaw(loc orig, loc tar)
	{
	 double dx = tar.x - orig.x; // diferença no eixo X (target menos origem)
	 double dy = orig.y - tar.y; // inverte o eixo Y

	 double ang = Math.Atan2(dx, dy) * (180.0 / Math.PI); // calcula ângulo anti-horário
	 if (ang < 0) ang += 360; // ajusta negativo para escala 0–360

	 double yaw = 360 - ang; // inverte o yaw visual (espelhamento em torno de 0)
	 if (yaw >= 360) yaw -= 360; // ajusta caso passe de 360

	 return (int)Math.Round(yaw); // retorna yaw como inteiro


	}
	// ------------------------------------------
	// MÉTODO giraface(facing) - Gira até o ângulo desejado
	// ------------------------------------------
	void giraface(int f1, int d = 99999)
	{
	 checkme();                              // atualiza posição e facing atual
	 int f0 = me.facing;                     // facing atual
	 bool correndo = isdown(WKEY); // esta correndo 
	 
	 int deltaR = (f0 - f1 + 360) % 360;     // ângulo pra direita
	 int deltaL = (f1 - f0 + 360) % 360;     // ângulo pra esquerda

	 bool direita = deltaR <= deltaL;        // decide direção
	 int delta = direita ? deltaR : deltaL;  // menor ângulo
	 bool logar = true; // flag para logar o giro em movimento
	 int tempo = Math.Min((int)(delta * 5.55), 1000); // tempo proporcional
	 float fator_drift = 1.25f; // fator de drift (ajuste fino)
	 if (me.spd > 3) tempo = (int)(tempo * fator_drift);      // aplica fator pra compensar o deslocamento lateral

	 const byte BYTE_AKEY = 0x41;
	 const byte BYTE_DKEY = 0x44;
	 byte tecla = direita ? BYTE_DKEY : BYTE_AKEY;    // usa valor literal: DKEY (0x44) ou AKEY (0x41)
	 if ((d < 200 && tempo > 350) || (d< 120 && tempo > 150)) // alvo muito perto e curva abrupta ORIGINAL ERA 200. TESTE DIMINUI TEMPO PARA 150
	 {
		solta(WKEY); // melhor parar de correr senao vai passar reto 
		//if (cb_log.Checked) loga($"Giro parado: Dist {d}m {tempo * 0.18} graus");
		logar = false;
	 }

   press(tecla);       // pressiona tecla de giro
	 wait(tempo);        // segura tempo proporcional
	 solta(tecla);       // solta tecla de giro

	 if (cb_log.Checked)
	 {
		checkme(); // atualiza posição e facing após o giro

		int f2 = me.facing; // facing final
		int erro = ((f2 - f1 + 540) % 360) - 180; // erro com sinal
		if (!direita) erro = -erro; // ajusta sinal conforme direção
		int delta_total = delta; // salva delta original (pra usar abaixo)

	
	 }


	 if (correndo) press(WKEY);



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
	// --------------------------------  
	// MÉTODO ANDA  
	// faz o bot andar pra frente por X metros, ou apenas iniciar/parar marcha  
	// --------------------------------  
	void anda(float m)
	{
	 if (m == 0) // se for 0, apenas começa a andar  
	 {
		press(WKEY); // segura pra frente  
		loga("iniciou marcha contínua");
		return;
	 }

	 if (m == -1) // se for -1, para de andar  
	 {
		solta(WKEY); // solta a tecla  
		loga("parou de andar");
		return;
	 }

	 if (m < 0) // não aceita valores negativos exceto -1  
	 {
		loga("erro: valor inválido para anda()");
		return;
	 }

	 float vel = 6f; // metros por segundo  
	 int tempo = (int)(m / vel * 1000); // tempo em ms = distância / velocidade  

	 press(WKEY); // começa a andar  
	 wait(tempo); // espera o tempo necessário  
	 solta(WKEY); // para de andar  
	 loga("andou " + m + " metros");
	}

	private void bt_debug2_Click(object sender, EventArgs e)
{
	 checkme();
tb_debug1.Text = me.pos.x.ToString();
tb_debug2.Text = me.pos.y.ToString();
tb_debug3.Text = me.facing.ToString();
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
	// MÉTODO CASTA: aperta e espera o GCD 
	//-----------------------------------------------------------
	void casta(byte key)
	{
	 aperta(key, GCD); // aperta a tecla e espera o GCD
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
on = false;
}

private void button4_Click(object sender, EventArgs e)
{
Clipboard.SetText(lb_log.Text); // copia todo o texto para a área de transferência
loga("Log copiado para área de transferência.\r\n"); // confirma no próprio log
	 lb_log.Text = ""; // limpa
}

	// --------------------------------  
	// MÉTODO UNSTUCK  
	// tenta pulo, testa se funcionou, senão gira e anda  
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
	// TIMERS 

	 int hs_tick = 0; // guarda o último tick registrado (global ou da classe)

	 if (cb_HS_timer.Checked && hs_tick == 0) // só na primeira vez
	 {
		hs_tick = Environment.TickCount; // marca início do controle

		int horas = 0;
		int.TryParse(tb_timer_hours.Text, out horas); // lê quantas horas rodar

		DateTime fim = DateTime.Now.AddHours(horas); // calcula horário final
		hs_min_left.Text = fim.ToString("dd/MM/yy HH:mm"); // salva no textbox
	 }


	 on = true;                        // ativa o bot
	 lwp.Clear();                      // limpa lista de waypoints

	 foreach (var item in lbwp.Items) // carrega os waypoints da listbox
		lwp.Add(unpack(item.ToString()));

	 if (lwp.Count == 0) return;       // não faz nada se a lista estiver vazia

	 checkme();                        // atualiza posição do bot

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
		 if (cb_round.Checked && cb_nostop.Checked)
		 {
			indexAtual = 0;               // modo circular → volta ao início
			loga("Loop completo. Recomeçando do início.");
		 }
		 else if (cb_nostop.Checked)
		 {
			dir = -1;                     // modo linear → inverte direção
			indexAtual = lwp.Count - 2;   // volta pro penúltimo
			loga("Término do caminho. Voltando.");
		 }
		 else break;                       // modo sem nostop → encerra
		}
		else if (indexAtual < 0) // passou do início
		{
		 if (cb_round.Checked && cb_nostop.Checked)
		 {
			indexAtual = lwp.Count - 1;   // modo circular → vai pro final
			loga("Loop completo. Recomeçando do final.");
		 }
		 else if (cb_nostop.Checked)
		 {
			dir = +1;                     // modo linear → inverte direção
			indexAtual = 1;               // vai pro segundo
			loga("Término do caminho. Indo novamente.");
		 }
		 else break;                       // modo sem nostop → encerra
		}
	 }
	}


	// --------------------------------
	// CARREGAR WAYPOINTS AO ABRIR O PROGRAMA
	// --------------------------------
	private void carregar_waypoints()
	{
	 string nome = "waypoints.txt"; // nome padrão

	 if (File.Exists("discord.ini")) // se arquivo de config existir
	 {
		string[] linhascfg = File.ReadAllLines("discord.ini");
		if (linhascfg.Length > 0 && linhascfg[0].Trim() != "")
		 nome = linhascfg[0].Trim(); // usa nome do cfg
	 }

	 tb_filename.Text = nome; // exibe o nome usado no textbox

	 if (File.Exists(nome)) // tenta carregar o arquivo
	 {
		string[] linhas = File.ReadAllLines(nome);
		lbwp.Items.Clear();

		foreach (string linha in linhas)
		 lbwp.Items.Add(linha);

		if (cb_log.Checked) loga($"Waypoints carregados do arquivo {nome}");
	 }
	 else
	 {
		if (cb_log.Checked) loga($"Arquivo {nome} não encontrado");
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
	tb_debug4.Text = d.ToString(); // mostra a distância no debug4

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
lbwp.Items.Clear();
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

	 // -------------------------
	 // TENTA PONTO CENTRAL
	 // -------------------------
	 loc centro = new loc { x = cx, y = cy };
	 mousemove(centro.x, centro.y); wait(pausa);
	 byte tipo = tipocur(); // obtém tipo do cursor atual

	 if (tipo == LOOT || (tipo == SKIN && cb_skinning.Checked) || (tipo == HERB && catando_planta))
	 {
		last_success = centro; total_loots++;
		logastats(); lootCombates++;

		if (tipo == 64) loga("Realizando loot.");
		else if (tipo == 63) loga("Extraindo couro.");
		else if (tipo == 69) loga("Localizou planta.");

		return centro;
	 }

	 // -------------------------
	 // ORDENA POSIÇÕES 1 a 24 POR FREQUÊNCIA
	 // -------------------------
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
		tipo = tipocur(); // lê tipo do cursor nesse ponto

		if (tipo == LOOT || (tipo == SKIN && cb_skinning.Checked) || (tipo == HERB && catando_planta))
		{
		 atualiza_lootfreq(idx); // atualiza frequência (exceto centro)
		 last_success = p; total_loots++;
		 logastats(); lootCombates++;

		 if (tipo == 64) loga("Localizou loot.");
		 else if (tipo == 63) loga("Localizou couro.");
		 else if (tipo == 69) loga("Localizou planta.");

		 return p;
		}
	 }

	 return new loc { x = -1, y = -1 }; // loot não encontrado
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

	


	// --------------------------------
	// BOTÃO SALVAR WAYPOINTS
	// --------------------------------
	private void bt_saveWP_Click(object sender, EventArgs e)
	{
	 if (lbwp.Items.Count > 0)
	 {
		List<string> linhas = new List<string>();

		foreach (var item in lbwp.Items)
		 linhas.Add(item.ToString());

		string nome = tb_filename.Text.Trim();
		if (nome == "") nome = "waypoints.txt";
		else if (!nome.Contains(".")) nome += ".txt";

		File.WriteAllLines(nome, linhas);        // salva os waypoints
		File.WriteAllText("discord.ini", nome);  // grava só o nome no cfg

		if (cb_log.Checked) loga($"Waypoints salvos ({linhas.Count} itens) no arquivo {nome}");
	 }
	 else
	 {
		if (cb_log.Checked) loga("Nenhum waypoint para salvar");
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
	if (cb_log.Checked) loga("cb_anda desmarcado: parando o bot.");
	on = false; // para o loop principal
 }
}
else
{
 if (!on) // só se o bot estava parado
 {
	if (cb_log.Checked) loga("cb_anda marcado: reiniciando o bot.");
	on = true; // liga o bot
	button1_Click_1(null, null); // chama o botão StartBot
 }
}
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

	 

	 float zoom = 100f;
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
		 lb_yaw.Text = (360 - yaw_visual).ToString();       // yaw visual (linha verde)
		 lb_delta.Text = getyaw(me.pos, alvo).ToString();   // yaw real

		 g.DrawLine(penTracejada, 128, 128, tx, ty);
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
		 string nome = ofd.FileName;

		 string[] linhas = File.ReadAllLines(nome);
		 lbwp.Items.Clear();

		 foreach (string linha in linhas)
			lbwp.Items.Add(linha);

		 // Atualiza textbox com nome limpo (sem path)
		 tb_filename.Text = Path.GetFileName(nome);

		 // Atualiza o discord.cfg com o novo nome
		 File.WriteAllText("discord.ini", tb_filename.Text.Trim());

		 if (cb_log.Checked) loga($"Waypoints carregados de {tb_filename.Text} ({linhas.Length} itens)");
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

	// --------------------------------------------
	// BOTÃO 14: LOOP DE CAPTURA DO MINIMAPA
	// --------------------------------------------
	private void button14_Click(object sender, EventArgs e)
	{
	 while (on) // enquanto estiver ativado
	 {
		get_minimap(); // chama método novo
		checkme();     // atualiza dados do jogador

		if (!on) break; // se desativado, encerra
	 }
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
	// BOTAO DEBUG - DUMP COMPLETO DO PALATABLE
	// Mostra todos os campos e estados do objeto pala
	// ----------------------------------------
	private void button17_Click(object sender, EventArgs e)
	{
	 checkme();
	 // -------------------------------------------------
	 // LOGA STATUS DE STEALTH DO ROGUE
	 // -------------------------------------------------
	 string s1 = rog.stealth_up ? "pode stealth" : "não pode stealth"; // se pode ativar
	 string s2 = rog.stealth ? "em stealth" : "fora de stealth";       // se está stealth
	 loga("Status Stealth: " + s1 + " / " + s2); // loga mensagem combinada
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
	 checkme();
	 wait(GCD); // espera o GCD
	 checkme(); // atualiza status do player
	 string aura_log = "[AURAS] "
 + "crus=" + pala.crusader + "  "
 + "devo=" + pala.devotion + "  "
 + "frost=" + pala.frost + "  "
 + "shadow=" + pala.shadow + "  "
 + "fire=" + pala.fire + "  "
 + "conc=" + pala.concentration + "  "
 + "retri=" + pala.retribution;

	 loga(aura_log); // ou tb_debug1.Text = aura_log;
	 loga(me.classe.ToString()); // mostra a classe do player
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
 }
}
