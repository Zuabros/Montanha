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
	bool on = true; // bot on
	public bool stopscan = false;
	int indexAtual = 0;



	// DLL Imports
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
	public const int BARRA = 0x6E; // tecla BARRA do NumPad

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
	public const int CONAURA = F8;     // Concentration Aura
	public const int BOP = F7;     // Retribution Aura


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

	// --------------------------------------------
	// CLASSES 
	// --------------------------------------------
	public const int PALADIN = 1; // Paladin

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
	 //executa_simulacao();


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
	 // R: Bitflags de status do player e do alvo
	 //     128 = autoattack ativo
	 //      64 = healing potion pronta (no inventário e cooldown zerado)
	 //      32 = target morto
	 //      16 = player está com debuff Dazed
	 //      0–15 = (livres para uso futuro)
	 // G: Proporção do HP do player (HP atual / HP máx × 255)
	 // B: (vazio – reservado para uso futuro)

	 //1 Player Mana:
	 //   R: Slots livres nas bags (255-free slots)
	 //   G: Facing wrong way = 255 / Out of range = 100 / 0 = normal. 
	 //   B: Mana atual / Mana máx × 255

	 //2 Player X posição:
	 //   R: (parte inteira de X / 10) × 25
	 //   G: parte decimal de X × 2.5 (até 100)
	 //   B: (parte inteira de X % 10) × 25

	 //3 Player Y posição:
	 //   R: (parte inteira de Y / 10) × 25
	 //   G: parte decimal de Y × 2.5 (até 100)
	 //   B: (parte inteira de Y % 10) × 25

	 // 4 Facing e Velocidade:
	 //   R: Facing normalizado (0 a 1) × 255
	 //   G: 255 - Velocidade atual (em unidades do jogo)
	 //   B: Bits combinados:
	 //      bits 0–5: Cooldown do Stoneform (0 a 63 segundos)
	 //      bit 6  (64): Racial disponível (Stoneform ou Shadowmeld)
	 //      bit 7 (128): Buff "Furbolg Form" ativo


	 //5 Status de Combate:
	 //   R: 0 = fora de combate, 255 se em combate
	 //   G: 255 se nadando na agua
	 //   B: Tipos de debuffs no char (somatório vestibular x 8)

	 //6 Player Level e Classe:
	 //   R: Level × 4 (cap em 255)
	 //   G: 255 se classe for Paladino
	 //   B: Bubble ready (B == 0)

	 //7 Target HP:
	 //   R: HP atual do alvo × 255 / HP máx
	 //   G: bits do target → 128 se existe, 64 se skinável
	 //   B: Level do alvo × 4 (cap em 255)

	 //8 vazio 
	 //   R: 0 
	 //   G: 0
	 //   B: 0

	 // -------------------------------------
	 // PIXEL 9 – JUDGEMENT + MELEE + AURAS
	 // -------------------------------------
	 // R: 255 se Judgement estiver em alcance
	 // G: Cooldown restante de Judgement (0–255, proporcional)
	 // B: Bitflags combinados:
	 //      1 = em melee range
	 //      2 = Crusader Aura
	 //      4 = Devotion Aura
	 //      8 = Frost Resist | 16 = Shadow Resist | 32 = Fire Resist
	 //      64 = Concentration | 128 = Retribution

	 //10 Cast do Player:
	 //   R: 255 se castando
	 //   G: Progresso da barra de cast (0–255)
	 //   B: ASCII da primeira letra da magia

	 //11 Cast do Target:
	 //   R: 255 se castando
	 //   G: Progresso da barra de cast (0–255)
	 //   B: ASCII da primeira letra da magia

	 // -------------------------------------
	 // PIXEL 12 – SEALS + JUDGEMENTS + CASTS
	 // -------------------------------------
	 // R: Bitflags dos seals ativos em você
	 //      128 = SOR | 64 = SOTC | 32 = SOJ | 16 = SOL | 8 = SOW | 4 = SOC
	 // G: Bitflags dos judgements no target (apenas os que aplicam debuff)
	 //      128 = Forbearance em mim; 64 = JOTC | 32 = JOJ | 16 = JOL | 8 = JOW
	 // B: Bitflags de cast disponível
	 //      128 = pode castar Lay on Hands
	 //       64 = pode castar Blessing of Protection

	 // -------------------------------------
	 // PIXEL 13 – BLESSINGS + HOJ STATUS
	 // -------------------------------------
	 // R: Bitflags dos blessings ativos em você
	 //      1 = Salvation | 2 = Light | 4 = Freedom | 8 = Wisdom
	 //      16 = Protection | 32 = Kings | 64 = Might | 128 = Sanctuary
	 // G: 128 se Hammer of Justice estiver pronto (CD 0), senão 0
	 // B: 255 se Hammer of Justice estiver em alcance do target
	 // -------------------------------------
	 // PIXEL 14 - TIPO DA CRIATURA DO TARGET
	 // -------------------------------------
	 // R: tipo da criatura ou player mané (codificado)
	 //      50  = Humanoide
	 //     100  = Besta
	 //     105  = Player mané ogro (não-caster)
	 //     110  = Player mané caster
	 //     150  = Morto-vivo
	 //     200  = Demônio
	 //     210  = Elemental
	 //     220  = Mecânico
	 //     230  = Dragonete
	 //     240  = Gigante
	 //      80  = Criatura pequena (Critter)
	 // G: livre
	 // B: livre

	 //15 Reação e Ameaça:
	 //   R: 255 se alvo for hostil
	 //   G: 255 se alvo for amigável
	 //   R+G = 255/255 se neutro
	 //   B: 255 se com ameaça máxima (aggro fixado)
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
	 // PIXEL 0 – HP, Autoattack, Potion, Target morto
	 // ----------------------------------------

	 // converte G em % de vida do player
	 int v_hp = (pixels[0].g * 100) / 255;
	 e.hp = v_hp;

	 // Tenho debuff dazed = bit 16
	 me.dazed = (pixels[0].r & 16) != 0;

	 // target morto agora vem no bit 32 de R, não mais no B
	 e.morreu = (pixels[0].r & 32) != 0;

	 // autoattack = bit 128
	 e.autoattack = (pixels[0].r & 128) != 0;

	 // nova flag: healing potion pronta = bit 64
	 e.hp_potion_rdy = (pixels[0].r & 64) != 0;

	 // debug opcional: mostra HP no textbox
	 tb_hp.Text = v_hp.ToString();



	 // ------------------------------------------
	 // Pixel 1 - Mana (canal B) / Bags (canal R) / Erros de combate (canal G)
	 // ------------------------------------------

	 // MANA
	 int v_mana = (pixels[1].b * 100) / 255;           // canal B (0–255) convertido em porcentagem
	 e.mana = v_mana;                                  // atualiza atributo de mana
	 tb_mana.Text = v_mana.ToString();                 // exibe no textbox (debug)

	 // SLOTS LIVRES NAS BAGS
	 int v_slots = 255 - pixels[1].r;                  // canal R invertido → slots livres reais
	 e.freeslots = v_slots;                            // atualiza slots livres

	 // ERROS DE COMBATE (bitmask no canal G)
	 int g = pixels[1].g;                              // canal G codifica erros combinados
	 e.wrongway = (g & 128) > 0;                     // 128 = "You are facing the wrong way!"
	 e.outofrange = (g & 64) > 0;                     //  64 = "Out of range" ou "You are too far away!"
	 
	 


	 // ------------------------------------------
	 // Pixel 3 e 4
	 // ------------------------------------------

	 // X
	 int dez_x = (int)Math.Round(pixels[2].r / 25.0);     // R = dezena × 25
	 int uni_x = (int)Math.Round(pixels[2].b / 25.0);     // B = unidade × 25
	 int dec_x = (int)Math.Round(pixels[2].g / 2.5);      // G = decimal × 2.5
	 int final_x = (dez_x * 10 + uni_x) * 100 + dec_x;    // X = (dezena*10 + unidade)*100 + decimal
	 e.pos.x = final_x;
	 tb_x.Text = final_x.ToString(); // debug opcional

	 // Y
	 int dez_y = (int)Math.Round(pixels[3].r / 25.0);     // R = dezena × 25
	 int uni_y = (int)Math.Round(pixels[3].b / 25.0);     // B = unidade × 25
	 int dec_y = (int)Math.Round(pixels[3].g / 2.5);      // G = decimal × 2.5
	 int final_y = (dez_y * 10 + uni_y) * 100 + dec_y;    // Y = (dezena*10 + unidade)*100 + decimal
	 e.pos.y = final_y;
	 tb_y.Text = final_y.ToString(); // debug opcional


	 // PIXEL 4 - facing (yaw) em W e velocidade (spd)
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
		var b = pixels[4].b;
		int sform_cd = b & 63;                    // bits 0–5 = cooldown da Stoneform (0–63)
		bool racial_up = (b & 64) != 0;           // bit 6 = racial disponível
		bool furbolg = (b & 128) != 0;            // bit 7 = buff Furbolg ativo

		if (cb_dwarf.Checked)                     // se for anão, usa racial como Stoneform
		 e.racialready = racial_up;

		e.furbolg_form = cb_furbolg.Checked && furbolg; // só ativa se checkbox estiver marcada
	 }


	 // ----------------------------------------------
	 // combate, nadando e debuffs (pixel 5)
	 // ----------------------------------------------
	 if (pixels.Count > 5)
	 {
		e.combat = pixels[5].r > 250; // está em combate se vermelho alto
		cb_combat.Checked = e.combat; // atualiza checkbox na UI

		e.swim = pixels[5].g > 250; // está se afogando se canal verde for 255

		int debuff_raw = pixels[5].b / 8; // extrai valor base dos debuffs (0–31)

		e.hascurse = (debuff_raw & 1) != 0;   // bit 0 → Curse
		e.hasother = (debuff_raw & 2) != 0;   // bit 1 → outro debuff (sem dispelType)
		e.haspoison = (debuff_raw & 4) != 0;  // bit 2 → Poison
		e.hasdisease = (debuff_raw & 8) != 0; // bit 3 → Disease
		e.hasmagic = (debuff_raw & 16) != 0;  // bit 4 → Magic
	 }

	 // -------------------------------------
	 // Pixel 6: Player Level, Classe, Flags
	 // -------------------------------------
	 if (pixels.Count > 6)
	 {
		int val = pixels[6].b; // canal B codifica flags

		// FLAGS DO PALADINO (bitfield)
		pala.bubble_cd = (val & 128) > 0;            // bolha disponível
		pala.exorcism_up = (val & 64) > 0; // true se exorcism está disponível
		pala.joj = (val & 32) > 0;             // mob com debuff JoJ
		pala.exorcism_range = (val & 16) > 0;             // exorcism em alcance
		//cb_exorange.Checked = pala.exorcism_range; // preenche a checkbox

		// LEVEL E CLASSE
		e.level = (int)Math.Round(pixels[6].r / 4.0);
		tb_level.Text = e.level.ToString();

		bool isPaladino = (pixels[6].g >= 250) || true;
		tb_class.Text = isPaladino ? "Paladino" : "Outro";
		if (isPaladino) e.classe = 1;
	 }


	 // -------------------------------------
	 // Pixel 7: Target HP, Flags, Level
	 // -------------------------------------
	 if (pixels.Count > 7)
	 {
		tar.hp = (pixels[7].r * 100) / 255;                // R: HP do alvo (%)
		tar.morreu = tar.hp == 0;                 // morreu se HP = 0
		tb_tarhp.Text = tar.hp.ToString();                 // mostra na textbox

		e.hastarget = (pixels[7].g & 128) > 0;             // G: bit 7 → existe target
		//tar.skinnable = (pixels[7].g & 64) > 0;            // G: bit 6 → skinável
		//if (tar.morreu && me.hastarget) loga($"Target skinnable: {tar.skinnable}"); // debug 

		tar.level = (int)Math.Round(pixels[7].b / 4.0);    // B: level do target (×4)
		//loga($"Level do target: {e.level}"); // debug
		tb_tarlevel.Text = tar.level.ToString();           // mostra na textbox
	 }

	 // -------------------------------------
	 // Pixel 8: Livre (Radar)
	 // -------------------------------------
	 if (pixels.Count > 8)
	 {
		// ainda não usado — radar será decodificado aqui depois
	 }

	 // -------------------------------------
	 // Pixel 9: Judgement (alcance, cooldown) + Auras + Melee range
	 // -------------------------------------
	 if (pixels.Count > 9)
	 {
		// canal R → está em alcance
		pala.jud_range = (pixels[9].r > 250);           // Judgement em alcance

		// canal G → cooldown restante (0–255)
		pala.judge_cd = pixels[9].g;                    // cooldown em segundos (cap 255)

		// canal B → decodifica bitflags de aura e melee range
		int b = pixels[9].b;                            // lê valor bruto (0–255)
		e.meleerange = (b & 1) != 0;            // bit 0 = melee range
		pala.crusader = (b & 2) != 0;            // bit 1 = Crusader Aura
		pala.devotion = (b & 4) != 0;            // bit 2 = Devotion Aura
		pala.frost = (b & 8) != 0;            // bit 3 = Frost Resist Aura
		pala.shadow = (b & 16) != 0;            // bit 4 = Shadow Resist Aura
		pala.fire = (b & 32) != 0;            // bit 5 = Fire Resist Aura
		pala.concentration = (b & 64) != 0;            // bit 6 = Concentration Aura
		pala.retribution = (b & 128) != 0;            // bit 7 = Retribution Aura

		cb_melee.Checked = e.meleerange;                // marca checkbox se em alcance físico
	 }


	 // Pixel 10: Cast do Player
	 // -------------------------------------
	 if (pixels.Count > 10)
	 {
		e.casting = pixels[10].r > 250;                           // está castando se canal R for 255
		e.castbar = (pixels[10].g * 100) / 255;                   // progresso em %
		e.spell = ((char)pixels[10].b).ToString();               // converte byte para letra
		e.spellid = 0;                                            // ainda não disponível

		tb_playercast.Text = me.casting ? me.spell : "-";         // exibe a letra da spell
		pb_playercast.Value = me.castbar;                         // atualiza progressbar (0-100)
	 }
	 
	 // Pixel 11: Cast do Target
	 // -------------------------------------
	 if (pixels.Count > 11)
	 {
		tar.castbar = (pixels[11].g * 100) / 255;                  // progresso em %
		tar.casting = tar.castbar > 0; // pixels[11].r > 250;                          // está castando se canal R = 255

		tar.spell = ((char)pixels[11].b).ToString();              // converte byte p
																															// ara letra
		tar.spellid = 0;                                           // ainda não disponível

		tb_tarcast.Text = tar.casting ? tar.spell : "-";          // mostra spell no textbox
		pb_tarcast.Value = tar.castbar;                           // atualiza progressbar
	 }

	 // -------------------------------------
	 // Pixel 12: Seals ativos (R) + Judgements no target (G) + flags de cast e debuff (B)
	 // -------------------------------------
	 if (pixels.Count > 12)
	 {
		int ar = pixels[12].r; // R = bitmask dos seals ativos
		int ag = pixels[12].g; // G = bitmask dos judgements no target
		int ab = pixels[12].b; // B = bitflags de cast e debuff

		// SEALS ATIVOS (true se bit correspondente estiver ligado)
		pala.sor = (ar & 128) != 0; // Seal of Righteousness
		pala.sotc = (ar & 64) != 0; // Seal of the Crusader
		pala.soc = (ar & 4) != 0; // Seal of Command
		pala.sol = (ar & 16) != 0; // Seal of Light
		pala.sow = (ar & 8) != 0; // Seal of Wisdom

		// JUDGEMENTS NO TARGET
		pala.jotc = (ag & 64) != 0; // Judgement of the Crusader
		pala.joj = (ag & 32) != 0; // Judgement of Justice
		pala.jol = (ag & 16) != 0; // Judgement of Light
		pala.jow = (ag & 8) != 0; // Judgement of Wisdom

		// FLAGS (CANCAST / DEBUFF)
		pala.cancast_LOH = (ab & 128) != 0; // pode castar Lay on Hands
		pala.BOP_up = (ab & 64) != 0; // pode castar Blessing of Protection
		pala.forbearance = (ag & 128) != 0; // tem debuff Forbearance
	 }


	 // -------------------------------------
	 // Pixel 13: Blessings ativos (R) + HoJ ready (G bit 7) + HoJ range (B)
	 // -------------------------------------
	 if (pixels.Count > 13)
	 {
		int ar = pixels[13].r; // R = bitmask dos blessings ativos
		int ag = pixels[13].g; // G = bitflags de cooldowns (bit 7 = HoJ pronto)
		int ab = pixels[13].b; // B = 255 se HoJ está em alcance

		// BLESSINGS ATIVOS (bit por ordem nova)
		pala.bos = (ar & 1) != 0; // bit 0 = Salvation
		pala.bol = (ar & 2) != 0; // bit 1 = Light
		pala.bof = (ar & 4) != 0; // bit 2 = Freedom
		pala.bow = (ar & 8) != 0; // bit 3 = Wisdom
		pala.bop = (ar & 16) != 0; // bit 4 = Protection
		pala.bok = (ar & 32) != 0; // bit 5 = Kings
		pala.bom = (ar & 64) != 0; // bit 6 = Might
		pala.bosanc = (ar & 128) != 0; // bit 7 = Sanctuary

		// HAMMER OF JUSTICE
		pala.hoj_ready = (ag & 128) != 0;        // está pronto
		pala.hoj_range = (ab > 250);             // está em alcance
		cb_hammer_range.Checked = pala.hoj_range;
	 }

	 // Pixel 14: Creature Type
	 // -------------------------------------
	 if (pixels.Count > 14)
	 {
		tar.type = pixels[14].r;               // lê valor bruto do canal R
		tb_tartype.Text = tar.type.ToString(); // exibe no textbox
	 }

	 // -------------------------------------
	 // Pixel 15: Mood (R/G) + Mob está me atacando? (B)
	 // -------------------------------------
	 if (pixels.Count > 15)
	 {
		// mood / reação do target
		bool hostile = pixels[15].r > 200 && pixels[15].g < 50;   // vermelho puro
		bool neutral = pixels[15].r > 200 && pixels[15].g > 200;  // amarelo
		bool friendly = pixels[15].r < 50 && pixels[15].g > 200;  // verde

		if (hostile) tar.mood = -1;
		else if (friendly) tar.mood = 1;
		else tar.mood = 0;

		tb_mood.Text = (tar.mood == -1) ? "Hostile" :
									 (tar.mood == 1) ? "Friendly" : "Neutral";

		// canal azul = 255 se o mob está me atacando (aggro confirmado)
		tar.aggroed = (pixels[15].b > 250);         // reuse da propriedade para "aggro ativo"
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
	
	// método separado que executa a simulação
	private void executa_simulacao()
	{
	 button7.Enabled = false;
	 loga("Iniciando testes. Aguarde.");

	 SimulaOtimos();

	 loga("Simulação finalizada.");
	 button7.Enabled = true;
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
	 Func<bool> has_seal = () => pala.sor || pala.soc || pala.sow || pala.sol || pala.sotc; // tem algum seal ativo

	 Func<int, bool> mana = (p) => me.mana > p;            // verifica se tem mana acima de p

	 getstats(ref me);                                     // atualiza status inicial
	 drawmap(destino);
	 if (cb_herbalism.Checked) atualizamapa(me.pos);
	 int temp = 0;                                         // contador de ciclos
	 loc oldloc = me.pos;                                  // guarda posição inicial para unstuck

	 Stopwatch timeout = new Stopwatch();                  // cronômetro para evitar travamento
	 timeout.Start();                                      // inicia contagem
	 if (cb_anda.Checked && Math.Abs(delta(me.pos, destino)) < 60)            // VIRA POUCO E MARCADO ANDA 
		press(WKEY); // ANDA 
	 else
		solta(WKEY); // PARA DE ANDAR 
	 long last = 0; // marca o tempo do último evento em milissegundos
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
		bless(me); // 

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
		 loga($"Distancia do alvo: {dist(me.pos, destino)}");
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
			if (cb_log.Checked) loga("enrosco lvl " + stuckcount);
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
		 puxa(pala);

		// -----------------------------
		// MORTE
		// -----------------------------
		//if (me.morreu)
		if (me.hp == 0) // morri
		{
		 aperta(WKEY, 2); // solta W
		 aperta(F5);      // botão de reset ou relog
		 if (cb_log.Checked) loga("Morreu! Bot parando.");
		 on = false;
		 return;
		}

		// -----------------------------
		// COMBATE
		// -----------------------------
		if (me.combat)
		{
		 aperta(WKEY, 2); // para de andar
		 if (cb_log.Checked) loga("Entrou em combate!");
		 combatloop(); // entra na rotina de combate


		 // -----------------------------------
		 // CHECK PÓS COMBATE 
		 // -----------------------------------
		 // SE PALADINO
		 //------------
		 // if paladino.... 

			if (me.hp < Convert.ToInt32(tb_preheal.Text) && mana(20))
		 {
			aperta(HLIGHT); wait(2500);
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
		//----------------------------------------------
		 if (me.mana < 60) loga($"Esperando recuperação da mana: {me.mana}");

		 while (me.mana < atoi(tb_pull_mana) && !me.combat) // espera recuperar mana
		 {
			if (!pala.bow) aperta(BOW); // buffa se não tiver buff ativo


			
			wait(1000);
			checkme();
		 }
		 // -----------------------------------
		 // LOOT 
		 // -----------------------------------
		 checkme();
		 if (cb_apagacinza.Checked) clica(new loc(5, 5), 1); // APAGA UM ITEM CINZA DA BAG, SE TIVER (botão criado pelo addon de apagar itens cinzas)
		 if (has(me.freeslots) || cb_loot_cloth.Checked) // só loota se estiver ativado, e se tiver espaço ou for loot de pano

		 {
			if (!me.combat)
			{


			 if (cb_loot.Checked)
			 {
				int loopcount = 0; // contador de tentativas de loot

				while (true)
				{
				 checkme();                  // atualiza dados antes de cada busca
				 if (me.combat) break;      // entrou em combate → sai do loop

				 if (loopcount++ >= 6)      // passou do limite → força saída
				 {
					loga("Loop de loot interrompido após 6 tentativas.");
					break;
				 }

				 loc p = scanloot();        // tenta encontrar algo clicável
				 if (p.x < 0) break;        // nada encontrado → encerra

				 clica(p);                  // clica com botão direito
				 wait(100);
				 checkme();

				 if (me.spd == 0 && !me.combat) // se continuou parado e sem combate
				 {
					wait(1200);                 // espera pelo loot ou skin
					if (cbskin.Checked) wait_cast(); // espera skin só se tiver ticado 
				 }
				 else
				 {
					loga("Clicou errado ao tentar loot.");
					break; // se clicou no mundo e saiu andando 
				 }


				}
			 }

			}
			checkme();
		 }
		 // -----------------------------------
		 // HEARTHSTONE SE BAGS FULL
		 // -----------------------------------
		 else if (cb_hearth_bagfull.Checked) // SEM ESPAÇO NA BAG - HEARTHSTONE ?
		 {
			HS(); // usa a pedra de lar se não tiver espaço na bag
		 }

		 //if (cb_anda.Checked) aperta(WKEY, 0); // retoma andar se permitido
		 return; // sai do moveto após combate. 
		}



	 } while (on && dist(me.pos, destino) > (catando_planta ? 70 : 110)); // enquanto ativo e longe (20 se catando)

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
	 aperta(HEARTHSTONE); // Casta hearth 
	 int segundos = 0; // timer para hearthstone 
	 loc origin = me.pos; // de onde eu saí 
	 loga("Bags full. Usando Hearthstone. ");
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






	// --------------------------------------------
	// MÉTODO puxa - Versão Paladino com verificação organizada
	// Inicia o combate apenas se o target for válido
	// --------------------------------------------
	void puxa(palatable pala)
	{
	 Func<bool> has_seal = () => pala.sor || pala.soc || pala.sow || pala.sol || pala.sotc; // verifica se tem algum Seal
	 Func<int, bool> mana = (p) => me.mana > p;
	 aperta(TAB);                                                               // seleciona inimigo próximo
	 checkme();                                                                 // atualiza status depois do tab

	 // NAO DEIXA AFOGAR 
	 //------------------------------
nao_afoga(); // nada para cima se estiver afogando


	 // --------------------------------------------
	 // VERIFICAÇÃO DO TARGET - bom ou ruim
	 // --------------------------------------------
	 bool good_target = true;                                                  // assume que o mob é válido

	 if (!me.hastarget)                                                        // sem alvo
		good_target = false;
	 else if (cb_pacifist.Checked)       // pacifista e não quer atacar
		good_target = false;                                                  
	 else if (tar.hp == 0)                                                     // mob já morto
		good_target = false;
	 else if (isgray(me.level, tar.level) && !cb_killgray.Checked)            // mob cinza e não queremos cinza
		good_target = false;
	 else if (tar.mood == 1)                                                  // mob amigável
		good_target = false;
	 else if (tar.combat)                                     // mob já em combate, e player não está
		good_target = false;
	 else if (tar.hp < 100)                                     // ja tem alguem batendo 
		good_target = false;
	 else if (tar.morreu)                                                     // mob já morreu
		good_target = false;
	 else if (tar.level > me.level + Convert.ToInt32(tb_pullcap.Text))       // mob acima do limite configurado
		good_target = false;
	 else if ((tar.type == 50 && cb_nohumanoid.Checked) ||                   // humanoide e checkbox ativa
						(tar.type == 230 && cb_nodragonkin.Checked))                   // dragonkin e checkbox ativa
		good_target = false;

	 if (!good_target || me.combat)                                                         // se não é um bom alvo
	 {
		aperta(CLEARTGT);                                                     // limpa o target
		return;                                                               // aborta o pull
	 }

	 // --------------------------------------------
	 // CORRE ATE O MOB E ATACA (PULL)
	 // --------------------------------------------
	 int ticker = 0; // contador de ciclos
	 
	 while (me.hastarget && !me.combat && tar.hp==100) // alvo válido e ainda fora de combate
	 {
		checkme();

		// NAO DEIXA AFOGAR 
		//------------------------------
		nao_afoga(); // nada para cima se estiver afogando


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

		if (ticker > 10) // passou 5s e não entrou em combate
		{
		 aperta(CLEARTGT); // limpa target e aborta pull
		 break;
		}
	 }
	}

	// --------------------------------
	// MÉTODO COMBATLOOP - ROTINAS DE COMBATE
	// --------------------------------
	// Variáveis de controle
	long last_purify = 0; // armazena o último uso do purify em ms
	// --------------------------------

	public void combatloop()
	{
	 solta(ANDA); // PARA DE ANDAR
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
		
		if (me.dazed || (!dungeon && !ja_deu_backpedal && me.hp < 85))
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
			if (!me.dazed) aperta(SKEY, 1500);     // anda pra trás mantendo o facing
			if (me.dazed) aperta(SKEY, 1500);     // anda pra trás mantendo o facing
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
		 wait(100); // dá tempo do jogo atualizar
		 checkme(); // atualiza dados após espera

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
		 if (!dungeon && me.casting && me.spell.StartsWith("S")) // se está castando skinning no meio do combate 
		 {
			aperta(PULA);
			roda(150);                // gira pra manter face no inimigo
		 }
		 aperta(INTERACT);               // anda até o alvo se estiver longe
		 if (ticker%6==0) aperta(PULA); // da uns pulinhos a cada 3 ciclos pra pular a cerca ao ir atras do target 
		}

		if (true || me.classe == 1)           // no futuro separar cura só pra paladino ou classe que cura 
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
			 && pala.exorcism_range
			 && mana(8);
		 if (tar.hp <100 && tar.combat && tar.meleerange)

		 // se pode, usa EXORCISM no lugar do Judgement
		 if (pode_exorcism)
			aperta(EXORCISM);

		 //-----------------SOR-------------------
		 aperta(best_seal(), 500); // ativa o melhor seal automaticamente
																		 //----------------BLESSINGS----------------
		 bless(me);                      // aplica blessing se necessário
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

		
		checkme();

	 } while (me.combat); // FIM DO LOOP DE COMBATE 

	 //---------------------------------------------
	 // TERMINA O COMBATE 
	 // ---------------------------------------------

	 // PALADINO (reseta variaveis de combate) 
	 //----------------------------------------------
	 pala.defseal = false;     // volta a permitir uso de SOR
	 pala.defbless = false;    // libera BOK ou BOM de novo
	 pala.defaura = false;     // pode voltar pra Ret Aura
	 pala.nomana = false;      // desativa fixação de BOW
	 //----------------------------------------------------

	 int final = decay.End(me.hp);
	 tracker.Add(final);
	 tbavdecay.Text = tracker.Average.ToString();
	 tbdecay.Text = final.ToString(); // opcional: mostrar o valor final após a luta

	 string txt = final.ToString();
	 if (!lbdecay.Items.Contains(txt))
		lbdecay.Items.Add(txt);

	 while (lbdecay.Items.Count > 10)
		lbdecay.Items.RemoveAt(0);

	 emCombate = false;


	}
	// --------------------------------
	// MÉTODO DE CURA GERAL DO PALADINO
	// --------------------------------
	void tenta_curar()
	{
	 int limiar_loh = 30; // hp abaixo desse valor pode disparar LOH
	 int.TryParse(tb_loh_hp.Text, out limiar_loh); // pega valor da caixa de texto

	 int limiar_hp = atoi(tb_combatheal); // pega valor de hp considerado critico
	 if (pala.sol) limiar_hp -= 7; // se tem Seal of Light, permite segurar mais

	 string dbg = "";

	 dbg += "Forbearance..........: " + (pala.forbearance ? "tem" : "não tem") + "\r\n";
	 dbg += "Lay on Hands pronto..: " + (pala.cancast_LOH ? "sim" : "não") + "\r\n";
	 dbg += "BoP pronto...........: " + (pala.BOP_up ? "sim" : "não") + "\r\n";
	 dbg += "BoP ativo em você....: " + (pala.bop ? "sim" : "não") + "\r\n";
	 dbg += "Potion pronta........: " + (me.hp_potion_rdy ? "sim" : "não") + "\r\n";
	 dbg += "Decay estimado.......: " + tbdecay.Text + " hp/m" + "\r\n";

	 if (me.hp < limiar_hp) loga(dbg); // loga debug se hp estiver abaixo do limiar crítico


	 // ----------------------------------------
	 // CURA COMUM – HLIGHT COM PROTEÇÃO
	 // ----------------------------------------
	 if (me.hp < limiar_hp && me.mana > 20 && !pala.forbearance)
	 {
		bool usou_protecao = false;

		if (pala.bubble_cd && me.mana > 25)
		{
		 aperta(DPROT, GCD); // usa divine protection
		 loga("Curando com divine protection.");
		 usou_protecao = true;
		}
		else if (pala.BOP_up && cb_BOP.Checked && me.mana > 25)
		{
		 aperta(BOP, GCD); // usa blessing of protection
		 loga("Curando com blessing of protection.");
		 usou_protecao = true;
		}

		if (usou_protecao)
		{
		 aperta(HLIGHT, 1000); // casta HLIGHT
		 wait_cast();
		 checkme();
		 if (me.hp >= limiar_hp) return; // cura resolveu
		}
	 }
	 // NAO USOU PROTEÇÃO OU A CURA FOI POUCA.
	 // ----------------------------------------
	 // CURA CRÍTICA – POTION OU HLIGHT COM STUN
	 // ----------------------------------------
	 if (me.hp < limiar_hp)
	 {
		if (me.hp_potion_rdy)
		{
		 aperta(HEALTHPOTION); // usa poção se tiver pronta
		 loga("Curando com potion.");
		 wait(GCD);
		 checkme();
		 if (me.hp >= limiar_hp) return; // cura resolveu
		}
		else if (pala.hoj_ready && pala.hoj_range)
		{
		 aperta(HOJ, GCD); // stuna o alvo com HOJ
		 loga("Curando com hammer of justice.");
		 aperta(HLIGHT, 1000); // casta HLIGHT durante o stun
		 wait_cast();
		 checkme();
		 if (me.hp >= limiar_hp) return; // cura resolveu
		}
	 }

	 // ----------------------------------------
	 // CURA DE EMERGÊNCIA – LOH OU CAST SECO
	 // ----------------------------------------
	 bool pode_usar_loh =
		 me.hp < limiar_loh &&
		 !me.hp_potion_rdy && // não tem potion
		 !pala.BOP_up &&       // não tem BOP
		 !pala.bubble_cd &&    // não tem bubble
		 !pala.hoj_ready &&    // não tem stun
		 pala.cancast_LOH &&   // LOH disponível
		 cb_loh.Checked;

	 if (pode_usar_loh)
	 {
		aperta(LOH); // cura total com LOH
		loga("Emergência. Curando com lay on hands.");
		return;
	 }

	 // ---------------------------------------------
	 // ÚLTIMA TENTATIVA: CURA SEM PROTEÇÃO
	 // ---------------------------------------------
	 if (me.hp < limiar_hp && me.mana > 20) // vida baixa e mana viável
	 {
		bool trocou_aura = false;       // flag pra saber se trocou aura
		byte aura_antiga = 0;            // salva a aura anterior pra restaurar depois

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

		loga("For the light!");     // tentativa heroica
		aperta(HLIGHT, 1000);       // casta holy light
		wait_cast();                // espera o cast
		checkme();                  // atualiza status

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

	void wait_cast() { checkme(); while (me.casting) { checkme(); wait(125); } } 
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
		if (cb_log.Checked) loga($"Giro parado: Dist {d}m {tempo * 0.18} graus");
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
		loga($"andou {andou}m, pulo resolveu, movimento retomado");
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
	 anda(6);      // anda 6 metros em nova direção
	 roda(80); // ou seja resultado final é virar para a esquerda 
	 anda(6); // anda 6 metros para a esquerda 
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
	 int hs_tick = 0; // guarda o último tick registrado (global ou da classe)
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
		if (cb_HS_timer.Checked) // só executa se o timer estiver ligado
		{
		 if (hs_tick == 0) hs_tick = Environment.TickCount; // inicia controle se for a primeira vez
		 int agora = Environment.TickCount; // tempo atual em ms
		 if (agora - hs_tick >= 60000) // passou 60 segundos?
		 {
			hs_tick = agora; // atualiza o marcador
			int min = 0; // minutos restantes
			int.TryParse(hs_min_left.Text, out min); // tenta converter o texto
			if (min > 0) min--; // decrementa se positivo
			hs_min_left.Text = min.ToString(); // atualiza o textbox
			if (min == 0) HS(); // chama HS se chegou a zero
		 }
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
	int pausa = 35; // delay padrão entre movimentos do mouse
	loc last_success = new loc { x = -1, y = -1 }; // último ponto que deu loot
	double peso_otimo = 10.0; // valor padrão, será carregado do arquivo
	double exp_otimo = 1.0;   // idem
	DateTime prox_salva = DateTime.Now.AddMinutes(10); // inicial

	// ---------------------------------------
	// METODO TESTEPONTO: MOVE E VERIFICA CURSOR
	// ---------------------------------------
	private bool testeponto(loc p, IntPtr refcur)
	{
	 mousemove(p.x, p.y);   // move o mouse até o ponto
	 wait(pausa);           // aguarda tempo global
	 IntPtr atual = getcursor(); // lê o cursor
	 return atual != refcur;     // compara
	}

	// ---------------------------------------------
	// MÉTODO SCANLOOT: VERSÃO COM DESCARTE PARCIAL
	// ---------------------------------------------
	// Tenta pontos com maior frequência primeiro,
	// mas se lootfreq == 0, 70% de chance de ignorar.
	// Atualiza lootfreq com janela FIFO adaptativa.
	// ---------------------------------------------
	public loc scanloot()
	{
	 Random rnd = new Random(); // colocar no início da classe
	 if (DateTime.Now >= prox_salva)
	 {
		salva_loot(); // salva stats e aplica trim se necessário
		prox_salva = DateTime.Now.AddMinutes(10);
	 }

	 focawow(); // traz o WoW pra frente

	 int w = Screen.PrimaryScreen.Bounds.Width;
	 int h = Screen.PrimaryScreen.Bounds.Height;

	 int cx = w / 2;
	 int cy = (int)(h / 2 + h * 0.05);

	 mousemove(20, 20); wait(pausa);
	 mousemove(25, 25); wait(pausa);

	 IntPtr refcur = getcursor(); // forma original do cursor

	 List<int> ordem = Enumerable.Range(0, 25)
		 .OrderByDescending(i => lootfreq[i])
		 .ThenBy(i => i)
		 .ToList();

	 foreach (int idx in ordem)
	 {
		// regra nova: se frequência zero, 70% de chance de pular o ponto
		if (!catando_planta && lootfreq[idx] == 0 && rnd.NextDouble() < 0.70)
		 continue;

		loc p;

		if (idx == 0)
		{
		 p = new loc { x = cx, y = cy };
		}
		else if (idx >= 1 && idx <= 16)
		{
		 int r = (int)(h * 0.20);
		 double ang = Math.PI * (idx - 1) / 8.0;
		 int x = cx + (int)(Math.Cos(ang) * r);
		 int y = cy - (int)(Math.Sin(ang) * r);
		 p = new loc { x = x, y = y };
		}
		else
		{
		 int r = (int)(h * 0.10);
		 double ang = Math.PI * (idx - 17) / 4.0;
		 int x = cx + (int)(Math.Cos(ang) * r);
		 int y = cy - (int)(Math.Sin(ang) * r);
		 p = new loc { x = x, y = y };
		}

		if (testeponto(p, refcur))
		{
		 atualiza_lootfreq(idx);
		 last_success = p;
		 total_loots++;
		 logastats();
		 return p;
		}
	 }

	 return new loc { x = -1, y = -1 };
	}

	Queue<int> fila_fifo = new Queue<int>(); // fora do método, persistente


	// ----------------------------------------  
	// FUNÇÃO carrega_loot  
	// ----------------------------------------  
	// Lê loot.txt, inicializa lootfreq[] e fila_fifo  
	// Se não existir, cria arquivo com dados padrão  
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
		lootfreq[i] = int.Parse(partes1[i].Trim());

	 // reconstrói fila_fifo com os índices da segunda linha
	 fila_fifo.Clear();
	 string[] partes2 = dados[1].Split(',');
	 foreach (string s in partes2)
	 {
		if (int.TryParse(s.Trim(), out int idx) && idx >= 0 && idx < 25)
		 fila_fifo.Enqueue(idx);
	 }

	 // atualiza total_loots como soma da fila (opcional, mas mantido)
	 total_loots = fila_fifo.Count;
	 loga($"lootfreq carregada: {string.Join(",", lootfreq)}");
	 loga($"total_loots atualizado: {total_loots}");
	}


	// ----------------------------------------
	// FUNÇÃO ATUALIZA LOOTFREQ (janela FIFO adaptativa)
	// ----------------------------------------
	void atualiza_lootfreq(int pos)
	{
	 if (pos < 0 || pos >= 25) return; // ignora índices inválidos

	 if (fila_fifo.Count >= 500)
	 {
		int velho = fila_fifo.Dequeue();
		if (velho >= 0 && velho < 25) // segurança extra
		 lootfreq[velho]--;
	 }

	 fila_fifo.Enqueue(pos);
	 lootfreq[pos]++;
	}

	// ----------------------------------------
	// FUNÇÃO salva_loot
	// ----------------------------------------
	// Salva vetor lootfreq[25] e a fila_fifo no loot.txt
	// Aplica trim leve se algum valor (exceto posição 0) passar de 400
	// ----------------------------------------
	void salva_loot()
	{
	 string arq = "loot.txt";

	 // aplica trim leve se necessário (ignora pos 0 no cálculo)
	 int max = lootfreq.Skip(1).Take(24).Max();
	 if (max > 400)
	 {
		double fator = 400.0 / max;
		for (int i = 1; i < 25; i++)
		 lootfreq[i] = (int)(lootfreq[i] * fator);
	 }

	 // monta linha 1: lootfreq[25]
	 string linha1 = string.Join(",", lootfreq);

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

	// --------------------------------------------
	// MÉTODO SimulaOtimos
	// --------------------------------------------
	// Simulação de força bruta para encontrar peso_otimo e exp_otimo ideais
	// Usa seu vetor lootfreq atual como base para a simulação
	private void SimulaOtimos()
	{
	 int total_tests = int.Parse(tb_loot_tries.Text);  
	 double melhor_peso = 0.0;
	 double melhor_exp = 0.0;
	 double melhor_media = double.MaxValue;

	 double peso_min = 1.0;
	 double peso_max = 20.0;
	 double exp_min = 0.5;
	 double exp_max = 2.0;

	 int n = (int)Math.Sqrt(total_tests);

	 for (int i = 0; i < n; i++)
	 {
		double peso_teste = peso_min + (peso_max - peso_min) * i / (n - 1);

		for (int j = 0; j < n; j++)
		{
		 double exp_teste = exp_min + (exp_max - exp_min) * j / (n - 1);

		 double media = simula_media_tentativas(peso_teste, exp_teste);

		 if (media < melhor_media)
		 {
			melhor_media = media;
			melhor_peso = peso_teste;
			melhor_exp = exp_teste;
		 }
		}

		if (i % 10 == 0) wait(1);
		if (i % (n / 10) == 0)
		 loga($"Progresso da simulação: {(100 * i) / n}%");

	 }

	 peso_otimo = melhor_peso;
	 exp_otimo = melhor_exp;

	 loga($"Melhor peso_otimo = {melhor_peso:F3}, melhor exp_otimo = {melhor_exp:F3}, média tentativas = {melhor_media:F3}");
	}

	// --------------------------------------------
	// MÉTODO simula_media_tentativas (placeholder)
	// --------------------------------------------
	// Implementar a lógica da simulação aqui conforme seu modelo
	private double simula_media_tentativas(double peso_teste, double exp_teste)
	{
	 int simulacoes = 10000;  // número de rodadas simuladas para média
	 Random rnd = new Random();

	 // Calcula a soma das frequências ignorando posição 0 (central)
	 int soma_freq = lootfreq.Skip(1).Take(24).Sum();

	 // Gera vetor de probabilidades reais normalizadas para as 24 posições (1 a 24)
	 double[] probs = new double[24];
	 if (soma_freq == 0)
	 {
		for (int i = 0; i < 24; i++)
		 probs[i] = 1.0 / 24.0;
	 }
	 else
	 {
		for (int i = 0; i < 24; i++)
		 probs[i] = (double)lootfreq[i + 1] / soma_freq;
	 }

	 double soma_tentativas = 0;

	 for (int rodada = 0; rodada < simulacoes; rodada++)
	 {
		// sorteia a posição correta de acordo com probs
		double val = rnd.NextDouble();
		double acumulado = 0;
		int pos_certa = 0;
		for (int i = 0; i < 24; i++)
		{
		 acumulado += probs[i];
		 if (val <= acumulado)
		 {
			pos_certa = i;
			break;
		 }
		}

		// calcula pesos adaptativos para todas as posições (1 a 24)
		double[] pesos = new double[24];
		double soma_pesos = 0;
		for (int i = 0; i < 24; i++)
		{
		 double chance = probs[i];
		 double peso_real = 1 + Math.Pow(chance, exp_teste) * peso_teste;
		 // aplica randomização no peso para simular o sorteio ponderado
		 pesos[i] = peso_real * (rnd.NextDouble() * 0.9 + 0.1); // entre 10% e 100% do peso_real
		 soma_pesos += pesos[i];
		}

		// simula o sorteio ponderado até encontrar a posição correta
		// para isso, ordena as posições por peso decrescente, e tenta uma a uma
		// ou simula o sorteio múltiplas vezes até achar pos_certa

		// Optamos por ordenar e contar a posição do pos_certa na ordem decrescente
		int tentativas = 1;
		int[] indices = Enumerable.Range(0, 24).ToArray();

		// ordena índices por peso decrescente
		Array.Sort(indices, (a, b) => pesos[b].CompareTo(pesos[a]));

		// conta quantas tentativas até achar pos_certa
		foreach (int idx in indices)
		{
		 if (idx == pos_certa)
			break;
		 tentativas++;
		}

		soma_tentativas += tentativas;
	 }

	 return soma_tentativas / simulacoes;
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
	// BOTAO DEBUG - TESTA FLAGS DO PIXEL 12
	// ----------------------------------------
	private void button17_Click(object sender, EventArgs e)
	{
	 checkme();
	 wait(GCD); // espera o GCD
	 bool trocou_aura = false;       // flag pra saber se trocou aura
	 byte aura_antiga = 0;            // salva a aura anterior pra restaurar depois

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

	 loga("For the light!");     // tentativa heroica
	 aperta(HLIGHT, 1000);       // casta holy light
	 wait_cast();                // espera o cast
	 checkme();                  // atualiza status

	 // restaura aura antiga se tiver trocado
	 if (trocou_aura && aura_antiga != 0)
	 {
		string nome_antiga = (aura_antiga == DEVAURA) ? "Devotion" : "Retribution";
		loga("Cura finalizada -> restaurando aura: " + nome_antiga);
		aperta(aura_antiga, GCD);
	 }
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
