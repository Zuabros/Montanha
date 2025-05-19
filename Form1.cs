// MÓDULO 01 - IMPORTAÇÕES
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices; // Required for Windows API functions
using System.Diagnostics;
using System.IO;
using System.Threading;
using static Discord.Form1;
using static Discord.funcoes;
using Discord; // garante acesso ao namespace da classe funcoes
using System.Security.Cryptography;
using System.Xml;
using System.Drawing.Imaging;
using System.CodeDom;

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
	// ALIAS SEMÂNTICOS - NOMES DAS SKILLS
	// --------------------------------------------
	public const int AUTOATTACK = UM;     // ataque automático
	public const int SOR = DOIS;   // Seal of Righteousness
	public const int BOM = TRES;   // Blessing of Might
	public const int JUDGEMENT = CINCO;  // Judgement
	public const int PURIFY = SEIS;   // remove poison/disease
	public const int CLEARTGT = SETE;   // limpa o target
	public const int HLIGHT = ZERO;   // Holy Light (cura)
	public const int DPROT = N1;     // Divine Protection
	public const int LOH = N2;       // Lay on Hands
	public const int BOW = N4;     // Blessing of Wisdom
	public const int EXORCISM = N6; // Exorcism no slot N6
	public const int HEARTHSTONE = N8;      // hearthstone
	public const int HEALTHPOTION = N9;      // hearthstone
	public const int MANAPOTION = N0;      // hearthstone
	public const int HOJ = QUATRO;   // Hammer of Justice
	public const int STONEFORM = NOVE;     // racial dos anões
	public const int ANDA = WKEY;     // anda

	// --------------------------------------------
	// UTILIDADES
	// --------------------------------------------
	public const int PULA = SPACEBAR;  // pula
	public const int INTERACT = IKEY;      // interage com o alvo
	

	// --------------------------------------------
	// F KEYS
	// --------------------------------------------
	public const int F1 = 0x70; // tecla F1
	public const int F2 = 0x71; // tecla F2 
	public const int F3 = 0x72; // tecla F3 
	public const int F4 = 0x73; // tecla F4 
	public const int F5 = 0x74; // tecla F5 
	public const int F6 = 0x75; // tecla F6 
	public const int F7 = 0x76; // tecla F7 
	public const int F8 = 0x77; // tecla F8 



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

	 carregar_waypoints(); // Chama o carregamento automático	 
	 tab_nav.SelectedIndex = 1; // Seleciona a tabPage2 (índice 1) por padrão - debug purposes

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

	 //0 Player HP / Autoattack / Target morto:
	 //   R: 255 se atacando automaticamente, senão 0
	 //   G: HP atual / HP máx × 255
	 //   B: 255 se o alvo estiver morto, senão 0

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

	 //4 Facing e Velocidade:
	 //   R: Facing normalizado (0 a 1) × 255
	 //   G: 255 - Velocidade atual (em unidades do jogo)
	 //   B: Cooldown Stoneform

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
	 //   G: 255 se existe target
	 //   B: Level do alvo × 4 (cap em 255)

	 //8 Blessings
	 //   R: 0 
	 //   G: 0
	 //   B: 0

	 //9 Julgamento e Melee:
	 //   R: 255 se Judgement estiver em alcance
	 //   G: Cooldown restante de Judgement (0–255, proporcional)
	 //   B: 255 se em melee range

	 //10 Cast do Player:
	 //   R: 255 se castando
	 //   G: Progresso da barra de cast (0–255)
	 //   B: ASCII da primeira letra da magia

	 //11 Cast do Target:
	 //   R: 255 se castando
	 //   G: Progresso da barra de cast (0–255)
	 //   B: ASCII da primeira letra da magia

	 //12 Buff Seal of Righteousness:
	 //   R: 255 se ativo
	 //   G: Segundos restantes × 4 (invertido, cap 255)
	 //   B: Lay on Hands (cd) 

	 // 13 Blessings / HoJ:
	 //   R: Bitflags de Blessings ativos (com bit 7 sempre ligado) → 128–255
	 //      Bit 0 = Might | 1 = Wisdom | 2 = Kings | 3 = Salvation
	 //      Bit 4 = Sanctuary | 5 = Freedom | 6 = Protection
	 //   G: Cooldown restante de Hammer of Justice (0–255)
	 //   B: 255 se Hammer of Justice estiver em alcance

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
	 // HP (green) e MORREU (blue=255)
	 int v_hp = (pixels[0].g * 100) / 255;          // G do pixel[0] convertido para % (vida do player)
	 e.hp = v_hp;                                   // atualiza hp do player
	 e.morreu = (pixels[0].b > 200);                // morreu se canal azul indica (target está morto)
	 tb_hp.Text = v_hp.ToString();                  // se debug ativo, mostra no textbox
	 e.autoattack = (pixels[0].r > 250);            // autoattack está ativado se R > 250
	 cb_autoattack.Checked = e.autoattack;          // atualiza checkbox de debug


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
	 cb_wrongway.Checked = e.wrongway;
	 


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
		e.facing = (int)Math.Round(yaw_raw); // converte para W (milésimos de pi-rad)
		if (cb_debug.Checked) tb_yaw.Text = e.facing.ToString(); // atualiza o textbox de yaw

		// velocidade
		e.spd = 255 - pixels[4].g; // inverte o valor do canal verde para obter a velocidade
		if (cb_debug.Checked) tb_spd.Text = e.spd.ToString(); // atualiza o textbox de velocidade

		if (cb_dwarf.Checked) // é anão? 
		{
		 // cooldown do Stoneform 
		 int sform_cd = pixels[4].b; // canal azul traz o valor em segundos (0–255)
		 e.racialready = (sform_cd == 0); // se cooldown for zero, racial está pronta
		}
	 }

	 // ----------------------------------------------
	 // combate, nadando e debuffs (pixel 5)
	 // ----------------------------------------------
	 if (pixels.Count > 5)
	 {
		e.combat = pixels[5].r > 250; // está em combate se vermelho alto
		cb_combat.Checked = e.combat; // atualiza checkbox na UI

		e.swim = pixels[5].g > 250; // está nadando se canal verde for 255

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
		if (isPaladino) me.classe = 1;
	 }


	 // -------------------------------------
	 // Pixel 7: Target HP, Existência, Level
	 // -------------------------------------
	 if (pixels.Count > 7)
	 {
		tar.hp = (pixels[7].r * 100) / 255;                 // R: HP do alvo (%)
		tb_tarhp.Text = tar.hp.ToString();                  // atualiza textbox de HP

		e.hastarget = pixels[7].g > 250;                    // G: existe target se for 255

		tar.level = (int)Math.Round(pixels[7].b / 4.0);     // B: Level codificado (×4)
		tb_tarlevel.Text = tar.level.ToString();            // atualiza textbox de level
	 }

	 // -------------------------------------
	 // Pixel 8: Livre (Radar)
	 // -------------------------------------
	 if (pixels.Count > 8)
	 {
		// ainda não usado — radar será decodificado aqui depois
	 }

	 // -------------------------------------
	 // Pixel 9: Judgement (alcance, cooldown) + melee range
	 // -------------------------------------
	 if (pixels.Count > 9)
	 {
		// canal R → está em alcance
		pala.jud_range = (pixels[9].r > 250);           // Judgement em alcance
		

		// canal G → cooldown restante (0–255)
		pala.judge_cd = pixels[9].g;                     // cooldown em segundos (cap 255)
		

		// canal B → melee range
		tar.meleerange = (pixels[9].b > 200);            // alcance melee com o target
		cb_melee.Checked = tar.meleerange;               // marca checkbox só se estiver em alcance
	 }


	 // Pixel 10: Cast do Player
	 // -------------------------------------
	 if (pixels.Count > 10)
	 {
		me.casting = pixels[10].r > 250;                           // está castando se canal R for 255
		me.castbar = (pixels[10].g * 100) / 255;                   // progresso em %
		me.spell = ((char)pixels[10].b).ToString();               // converte byte para letra
		me.spellid = 0;                                            // ainda não disponível

		tb_playercast.Text = me.casting ? me.spell : "-";         // exibe a letra da spell
		pb_playercast.Value = me.castbar;                         // atualiza progressbar (0-100)
	 }
	 
	 // Pixel 11: Cast do Target
	 // -------------------------------------
	 if (pixels.Count > 11)
	 {
		tar.casting = pixels[11].r > 250;                          // está castando se canal R = 255
		tar.castbar = (pixels[11].g * 100) / 255;                  // progresso em %
		tar.spell = ((char)pixels[11].b).ToString();              // converte byte para letra
		tar.spellid = 0;                                           // ainda não disponível

		tb_tarcast.Text = tar.casting ? tar.spell : "-";          // mostra spell no textbox
		pb_tarcast.Value = tar.castbar;                           // atualiza progressbar
	 }

	 // -------------------------------------
	 // Pixel 12: Buff de Combate (Seal of Righteousness) + CD do Lay on Hands
	 // -------------------------------------
	 if (pixels.Count > 12)
	 {
		int ar = pixels[12].r;                      // canal R indica se Seal of Righteousness está ativo
		int ag = pixels[12].g;                      // canal G codifica tempo restante do Seal
		int ab = pixels[12].b;                      // canal B = cooldown restante do Lay on Hands (cap 255s)

		cb_hasSOR.Checked = (ar > 200);             // marca checkbox se buff está presente

		int tempo_restante = (255 - ag) / 4;        // converte canal G para segundos (duração do Seal)
		pala.sor = tempo_restante;                  // armazena tempo restante no objeto

		pala.loh_cd = ab;                           // armazena cooldown do Lay on Hands (0 a 255)
	 }

	 // -------------------------------------
	 // Pixel 13: Blessings ativos (RED) e HAMMER (G/B)
	 // -------------------------------------
	 if (pixels.Count > 13)
	 {
		int ar = pixels[13].r;                    // canal R = bitflags de blessings (com bit 7 sempre ligado)
		int ag = pixels[13].g;                    // canal G = cooldown do Hammer of Justice (0–255)
		int ab = pixels[13].b;                    // canal B = 255 se HoJ está em alcance

		// leitura dos bits de blessings
		pala.bom = (ar & 1) != 0; // bit 0 = Blessing of Might
		pala.bow = (ar & 2) != 0; // bit 1 = Blessing of Wisdom
		pala.bok = (ar & 4) != 0; // bit 2 = Blessing of Kings
		pala.bos = (ar & 8) != 0; // bit 3 = Blessing of Salvation
		pala.bof = (ar & 32) != 0; // bit 5 = Blessing of Freedom
		pala.bop = (ar & 64) != 0; // bit 6 = Blessing of Protection

		// hammer of justice
		pala.hoj_cd = ag;                         // cooldown do Hammer of Justice
		pala.hoj_range = (ab > 250);              // se está em alcance
		cb_hammer_range.Checked = pala.hoj_range; // marca se esta em range 
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



	// M01 - MÉTODO CLICA - MOVE O MOUSE PARA (X, Y) E REALIZA UM CLIQUE COM O BOTÃO INFORMADO.
	public void clica(int x, int y, int botao = 1)
{
mousemove(x, y);
DoMouseClick(botao);
}



	// --------------------------------
	// MÉTODO GIRALVO 5.2 - GIRA O PERSONAGEM PARA A COORDENADA ALVO (versão 0–360)
	// --------------------------------
	public void giralvo(loc alvo)
	{
	 giraface(getyaw(me.pos, alvo),dist(me.pos,alvo));
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


	// --------------------------------
	// MÓDULO 14 - MÉTODO MOVETO (MAIN LOOP)
	// Anda até destino, corrige direção, reage a combate, detecta morte e unstuck
	// --------------------------------
	public void moveto(loc destino)
	{
	 Func<bool> has_seal = () => has(pala.sor) || has(pala.soc) || has(pala.sow) || has(pala.sol) || has(pala.sotc); // tem algum seal
	 Func<int, bool> mana = (p) => me.mana > p;            // verifica se tem mana acima de p

	 getstats(ref me);                                     // atualiza status inicial
	 drawmap(destino);
	 atualizamapa(me.pos);
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
		if (!me.combat && tar.morreu)
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
		 lb_combat_count.Text = (int.Parse(lb_combat_count.Text) + 1).ToString(); // increase o mob count

		 // -----------------------------------
		 // CHECK PÓS COMBATE 
		 // -----------------------------------
		 if (me.hp < Convert.ToInt32(tb_preheal.Text) && mana(20))
		 {
			aperta(HLIGHT); wait(2500);
			bless(me);
		 }
		 if (me.mana < 60)
		 {
			if (me.mana < 50) aperta(N4); // Pala? BOW
			aperta(N3); // Bebe
			while (me.mana < 60 && !me.combat) // espera recuperar mana
			{
			 wait(1000);
			 checkme();
			}
		 }
		 // -----------------------------------
		 // LOOT 
		 // -----------------------------------
		 checkme();
		 if (has(me.freeslots) || cb_loot_cloth.Checked) // só loota se estiver ativado, e se tiver espaço ou for loot de pano

		 {
			if (!me.combat)
			{
			 if (cb_loot.Checked)
				{
				aperta(SETE, 100); // clear target 
				aperta(IKEY, 1500); // LOOT
			 }
			}
			checkme();
		 }
		 // -----------------------------------
		 // HEARTHSTONE SE BAGS FULL
		 // -----------------------------------
		 else if (cb_hearth_bagfull.Checked) // SEM ESPAÇO NA BAG - HEARTHSTONE ?
		 {
			aperta(HEARTHSTONE); // Casta hearth 
			int segundos = 0; // timer para hearthstone 
			loc origin = me.pos; // de onde eu saí 
			loga("Bags full. Usando Hearthstone. ");
			while (!me.combat && segundos<20) // espera hearth 
			{
			 wait(1000);
			 checkme();
			 segundos++;
			}
			if (dist(me.pos,origin) > 200) // foi para bem longe 
			{
			 loga("Teleport confirmado, encerrando o bot");
			 Environment.Exit(0);
			}
		 }

		 //if (cb_anda.Checked) aperta(WKEY, 0); // retoma andar se permitido
		 return; // sai do moveto após combate. 
		}
		

		
	 }while (on && dist(me.pos, destino) > 100);        // enquanto ativo e longe do destino
	 loga("Alvo atingido. Partindo para proximo alvo.");
	 //aperta(WKEY, 2); // solta W ao chegar no destino
	}
	// --------------------------------------------
	// MÉTODO BLESS - Aplica o buff apropriado
	// --------------------------------------------
	public void bless(element e)
	{
	 checkme(); // atualiza status

	 // --------------------------------------------
	 // PRIORIDADE 1: Buffar outro player (do grupo)
	 // --------------------------------------------
	 if ((tar.type == 105 || tar.type == 110))         // se o alvo for player
	 {
		if (tar.type == 110)
		 aperta(BOW);                            // caster → Blessing of Wisdom
		else
		 aperta(BOM);                            // melee → Blessing of Might

		aperta(CLEARTGT);                            // limpa o target depois do buff
	 }

	 // --------------------------------------------
	 // PRIORIDADE 2: Buffar a si mesmo
	 // --------------------------------------------
	 int blessneeded = 0; // 0 = nada, 1 = BOM, 2 = BOW, 3 = BOK
	 int wiztrig = int.Parse(tb_bow_trig.Text); // lê o limiar de mana

	 if (me.level >= 20 && cb_BOK.Checked && mana(Math.Min(wiztrig + 30, 100)))                // quer Kings e tem mana suficiente
		blessneeded = 3;

	 else if (cb_BOM.Checked && mana(Math.Min(wiztrig + 30, 100)))
		blessneeded = 1;                            // quer Might se tiver mana

	 else if (me.level >= 14 && cb_BOW.Checked && mana(10) && !mana(wiztrig))
		blessneeded = 2;                            // quer Wisdom se com pouca mana

	 // --------------------------------------------
	 // Executa o buff conforme a necessidade
	 // --------------------------------------------
	 if (blessneeded == 3 && !pala.bok)
		aperta(N5);                                // aplica Blessing of Kings

	 else if (blessneeded == 1 && !pala.bom)
		aperta(BOM);                                // aplica Blessing of Might

	 else if (blessneeded == 2 && !pala.bow)
		aperta(BOW);                                // aplica Blessing of Wisdom
	}




	// --------------------------------------------
	// MÉTODO puxa - Versão Paladino com verificação organizada
	// Inicia o combate apenas se o target for válido
	// --------------------------------------------
	void puxa(palatable pala)
	{
	 Func<bool> has_seal = () => has(pala.sor) || has(pala.soc) || has(pala.sow) || has(pala.sol) || has(pala.sotc); // verifica se tem algum Seal
	 Func<int, bool> mana = (p) => me.mana > p;                                // verifica se tem mana suficiente

	 aperta(TAB);                                                               // seleciona inimigo próximo
	 checkme();                                                                 // atualiza status depois do tab

	 // --------------------------------------------
	 // VERIFICAÇÃO DO TARGET - bom ou ruim
	 // --------------------------------------------
	 bool good_target = true;                                                  // assume que o mob é válido

	 if (!me.hastarget)                                                        // sem alvo
		good_target = false;
	 else if (isgray(me.level, tar.level) && !cb_killgray.Checked)            // mob cinza e não queremos cinza
		good_target = false;
	 else if (tar.mood == 1)                                                  // mob amigável
		good_target = false;
	 else if (tar.combat && !me.combat)                                       // mob já em combate, e player não está
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
	 
	 while (me.hastarget && !me.combat && tar.hp > 0) // alvo válido e ainda fora de combate
	 {
		checkme();
		// aplica Seal of Righteousness se necessário
		if (!has(pala.sor) && mana(7))
		 aperta(SOR);

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
		Func<bool> has_seal = () => has(pala.sor) || has(pala.soc) || has(pala.sow) || has(pala.sol) || has(pala.sotc);
		Func<int, bool> mana = (p) => me.mana > p;

		if (me.combat) // calculo do decay
		{
		 decay.Update(me.hp);
		 tbdecay.Text = decay.Current.ToString();
		}


		// --------------------------------
		// ROTINAS ALL-CLASS
		// --------------------------------
		//----- HEALTH POTION --------
		if (me.hp < 30 && me.mana < 40) // Tá no bico do corvo 
		 aperta(HEALTHPOTION);
		else if (me.hp > 80 && me.mana < 15) // lay on hands ou drain mana
		 aperta(MANAPOTION);

		// -----------------------------------------
		// recuo tático se estiver apanhando demais
		// -----------------------------------------
		if (!ja_deu_backpedal && me.hp < 85)
		{
		 
		 int limiar = int.Parse(tb_back_limiar.Text); // Lê o valor do limiar 
		 int decay = int.Parse(tbdecay.Text);// Obtém os valores de decay e avg_decay 
		 int avg_decay = int.Parse(tbavdecay.Text);

		 // Executa a lógica apenas se o mob não estiver castando e o decay atual for maior que o limiar
		 if (!tar.casting && decay > limiar && tar.hp > 20)
		 {
			ja_deu_backpedal = true; // só uma vez por combate
			loga($"Dando Backpedal: decay = {int.Parse(tbdecay.Text)}");
			aperta(SKEY, 3000);     // anda pra trás mantendo o facing
			aperta(AUTOATTACK);


		 }
		 else if (!jalogou)
		 {
			loga($"Backpedal não necessário: decay = {int.Parse(tbdecay.Text)}");
			jalogou = true; // loga o decay apenas uma vez por combate
		 }
		}

		// --------------------------------
		// VERIFICA SE COMBATE TERMINOU
		// --------------------------------
		wait(100);
		checkme();
		if (!tar.aggroed) // target morto ou aparentemente inválido
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
		 if (me.casting && me.spell.StartsWith("S")) // se está castando skinning no meio do combate 
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
			if (cb_useSOR.Checked && mana(7) && !has(pala.sor)) // quer usar SOR, tem mana, e não tem o buff
			aperta(SOR, 500);         // ativa Seal of Righteousness
		 //----------------BLESSINGS----------------
		 bless(me);                      // aplica blessing se necessário
		 //---------------STONEFORM------------------

		 if (cb_dwarf.Checked && me.racialready)                     // é anão e o racial está pronto
		 {
			int limiar = Convert.ToInt32(tb_stoneform_at.Text);     // lê o valor do limiar de HP do textbox
			if (me.hp < limiar || me.hasother || me.haspoison || me.hasdisease)
			 aperta(STONEFORM);                                // ativa Stoneform se atender condições
		 }
		 else if (me.level >= 8 && cb_purify.Checked && (me.haspoison || me.hasdisease) && mana(20))
			aperta(PURIFY);                                         // se não for anão, usa Purify se puder

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

	 } while (me.combat); // REGISTRO DO DECAY 

	 int final = decay.End(me.hp);
	 tracker.Add(final);
	 tbavdecay.Text = tracker.Average.ToString();
	 tbdecay.Text = final.ToString(); // opcional: mostrar o valor final após a luta

	 string txt = final.ToString();
	 if (!lbdecay.Items.Contains(txt))
		lbdecay.Items.Add(txt);

	 while (lbdecay.Items.Count > 15)
		lbdecay.Items.RemoveAt(0);

	 emCombate = false;


	}


	// --------------------------------
	// MÉTODO DE CURA GERAL DO PALADINO
	// --------------------------------
	void tenta_curar()
	{
	 int limiar_loh = 15;
	 int.TryParse(tb_loh_hp.Text, out limiar_loh);

	 if (cb_loh.Checked && me.hp < limiar_loh && !has(pala.loh_cd))
	 {
		aperta(LOH);
		return;
	 }

	 if (me.hp < 40 && me.mana > 20) // vida baixa, mana boa 
	 {
		bool usou_bolha = false;

		if (pala.bubble_cd && me.level >= 6 && cb_bubble.Checked && me.mana > 35) // bolha up 
		{
		 aperta(DPROT, 1501); // casta bolha 
		 usou_bolha = true;
		}

		if (!usou_bolha) // nao castou bolha 
		{
		 if (pala.hoj_cd == 0 && pala.hoj_range) // tenta hammer 
			aperta(HOJ, 1500);  // casta 
		}
		aperta(HLIGHT, 1000); // cura 
		wait_cast(); // espera curar 
		checkme(); // castar 

		if (usou_bolha && me.hp < 80)
		{
		 aperta(HLIGHT, 1000);
		 wait_cast();
		}
	 }
	}

	// --------------------------------
	// VERIFICA SE DEVE USAR JUDGEMENT
	// --------------------------------
	bool should_judge()
	{
	 if (me.level < 4) return false;
	 if (!cb_judge.Checked) return false;
	 if ((tar.hp > 25 && !mana(15)) || (tar.hp <= 25 && !mana(4))) return false; // reserva manda pra curar 
	 if (!has(pala.sor) && !has(pala.sow) && !has(pala.sol) && !has(pala.soc) && !has(pala.sotc)) return false;
	 if (!pala.jud_range) return false;
	 if (pala.judge_cd != 0) return false;
	 return true;
	}

	// --------------------------------
	// VERIFICA SE DEVE USAR HAMMER OF JUSTICE
	// --------------------------------
	bool should_stun()
	{
	 if (me.level < 8) return false;
	 if (!cb_use_hammer.Checked) return false;
	 if (!mana(13)) return false;
	 if (pala.hoj_cd != 0) return false;
	 if (!pala.hoj_range) return false;

	 int limite = 60;
	 if (int.TryParse(tb_interrupt_at.Text, out int v) && v >= 1 && v <= 100)
		limite = v;

	 if (tar.casting)
	 {
		if (tar.castbar <= limite) return true;
		if (tar.spell.StartsWith("H")) return true;
	 }

	 if (me.hp < 40) return true;

	 return false;
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
	 int neardist = 100;
	 on = true;

	 loc go = nloc(int.Parse(tb_debug1.Text), int.Parse(tb_debug2.Text));

	 while (dist(me.pos,go) > neardist)
	 {
		checkme();

		if (!me.combat) giralvo(go);

		if (!on) return;

		
	 }
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

	 int tempo = Math.Min((int)(delta * 5.55), 1000); // tempo proporcional
	 if (me.spd > 3) tempo = (int)(tempo * 1.25f);      // aumenta se correndo

	 const byte BYTE_AKEY = 0x41;
	 const byte BYTE_DKEY = 0x44;
	 byte tecla = direita ? BYTE_DKEY : BYTE_AKEY;    // usa valor literal: DKEY (0x44) ou AKEY (0x41)
	 if (d < 200 && tempo > 200) // alvo muito perto e curva abrupta 
	 {
		solta(WKEY); // melhor parar de correr senao vai passar reto 
		if (cb_log.Checked) loga($"Freando para virar: Dist {d}m {tempo * 0.18} graus");
	 }

   press(tecla);       // pressiona tecla de giro
	 wait(tempo);        // segura tempo proporcional
	 solta(tecla);       // solta tecla de giro

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
		loga("girando " + ang + "° pra direita");
		press(DKEY); // pressiona tecla de girar pra direita  
		wait(tempo); // espera o tempo proporcional  
		solta(DKEY); // solta a tecla  
	 }
	 else // se ângulo for negativo, gira pra esquerda  
	 {
		loga("girando " + ang + "° pra esquerda");
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
	// BOTAO GIRALVO, LE COORDENADAS DAS TB E GIRA PARA ELAS. 
	private void bt_debug3_Click(object sender, EventArgs e)
	{
	 checkme();
	 loc go = nloc(int.Parse(tb_debug1.Text), int.Parse(tb_debug2.Text));
	 tb_debug3.Text = dist(me.pos, go).ToString() ;
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
	// Clica em ponto na tela 


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

	// --------------------------------
	// BOTÃO PERCORRE LISTA (COM NOSTOP)
	// Percorre waypoints em modo circular ou linear (de trás pra frente)
		// --------------------------------
	private void button1_Click_1(object sender, EventArgs e)
	{
	 on = true;                // ativa o bot
	 lwp.Clear();              // limpa lista de waypoints

	 foreach (var item in lbwp.Items)       // percorre itens da listbox
		lwp.Add(unpack(item.ToString())); // desempacota e adiciona na lista

	 if (lwp.Count == 0) return; // não faz nada se a lista estiver vazia

	 checkme(); // atualiza posição atual

	 List<int> ordem = new List<int>(); // ordem de execução dos pontos

	 if (cb_round.Checked) // modo circular
	 {
		int start = nearest(me.pos, lwp); // encontra o ponto mais próximo
		if (start == -1) return;

		for (int i = start; i < lwp.Count; i++) ordem.Add(i);  // do mais próximo até o fim
		for (int i = 0; i < start; i++) ordem.Add(i);          // e depois do início até antes do mais próximo
	 }
	 else // modo linear (inverso)
	 {
		for (int i = lwp.Count - 1; i >= 0; i--) ordem.Add(i); // vai de trás pra frente
	 }

	 while (on) // enquanto ativo
	 {
		int idx = ordem[indexAtual];         // índice atual
		lbwp.SelectedIndex = idx;            // destaca na interface

		if (dist(me.pos, lwp[idx]) < 1) continue;

		 moveto(lwp[idx]);                // usa método clássico 

		indexAtual++;                        // avança no índice

		if (indexAtual >= ordem.Count)      // se chegou no fim da lista
		{
		 if (cb_nostop.Checked)          // se for nostop
		 {
			if (!cb_round.Checked) ordem.Reverse(); // inverte ordem se não for circular
			indexAtual = 0;            // recomeça
		 }
		 else break;                     // senão, para
		}
	 }
	}


	// --------------------------------
	// CARREGAR WAYPOINTS AO ABRIR O PROGRAMA
	// --------------------------------
	private void carregar_waypoints()
	{
	 string nome = "waypoints.txt"; // nome padrão

	 if (File.Exists("discord.cfg")) // se arquivo de config existir
	 {
		string[] linhascfg = File.ReadAllLines("discord.cfg");
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

	//BOTAO DRAWLOOP
private void button7_Click(object sender, EventArgs e)
{
while (on)
	 {
		bt_debut3_Click(null, null);
		
		wait(300);
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

		foreach (var item in lbwp.Items)
		 linhas.Add(item.ToString());

		string nome = tb_filename.Text.Trim();
		if (nome == "") nome = "waypoints.txt";
		else if (!nome.Contains(".")) nome += ".txt";

		File.WriteAllLines(nome, linhas);        // salva os waypoints
		File.WriteAllText("discord.cfg", nome);  // grava só o nome no cfg

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
	// BOTÃO DRAWMAP
	// Gera e exibe imagem do mapa com as entidades visíveis
	// --------------------------------
	private void bt_debut3_Click(object sender, EventArgs e)
	{
	 checkme(); // atualiza player
							// o facing começa em zero e cresce no sentido anti-horario até 359
							// calcula yaw diretamente, sem usar getyaw()
	 wait(100);
	 loc alvo = new loc(int.Parse(tb_debug1.Text), int.Parse(tb_debug2.Text));

	 lb_delta.Text = getyaw(me.pos, alvo).ToString();//

	 double dx = alvo.x - me.pos.x;
	 double dy = alvo.y - me.pos.y;

	 double ang = Math.Atan2(dx, dy) * (180.0 / Math.PI); // angulo anti-horário
	 if (ang < 0) ang += 360;

	 int yaw = (int)Math.Round(ang);


	 points.Clear();
	 points.Add(new point(me.pos, 1)); // player
	 points.Add(new point(new loc(int.Parse(tb_debug1.Text), int.Parse(tb_debug2.Text)), 4));

	 float zoom = 100f;
	 float esc = 1.5f * (zoom / 100f); // 3 pixels por unidade (com base no sistema inteiro)

	 Bitmap bmp = new Bitmap(256, 256);

	 using (Graphics g = Graphics.FromImage(bmp))
	 {
		g.Clear(Color.Black);

		foreach (point p in points)
		{
		 int dxi = (int)((p.pos.x - me.pos.x) * esc); // X deslocado
		 int dyi = (int)((p.pos.y - me.pos.y) * esc); // Y deslocado

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
		// LINHA DE MIRADA
		double rad = me.facing * Math.PI / 180.0;
		int fx = 128 - (int)(Math.Sin(rad) * 20); // X já estava corrigido
		int fy = 128 - (int)(Math.Cos(rad) * 20); // Y agora corrigido
		g.DrawLine(Pens.White, 128, 128, fx, fy); // desenha a seta

		// LINHA VERDE TRACEJADA: direção ao target
		using (Pen penTracejada = new Pen(Color.Green, 1))
		{
		 penTracejada.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
		 int tx = 128 + (int)((alvo.x - me.pos.x) * esc);
		 int ty = 128 + (int)((alvo.y - me.pos.y) * esc);

		 // calcula yaw visual a partir da linha verde
		 dx = tx - 128;
		 dy = 128 - ty; // inverte o eixo Y visual

		 ang = Math.Atan2(dx, dy) * (180.0 / Math.PI);
		 if (ang < 0) ang += 360;

		 int yaw_visual = (int)Math.Round(ang); // ESSE É O VALOR DO YAW CORRETO QUE VOCE DEVE RETORNAR NA FUNÇAO getyaw(loc orig, loc alvo)
		 lb_yaw.Text = (360-yaw_visual).ToString(); // este valor esta certo 
		 
		 // desenha linha verde tracejada do player até o target - esta linha é o yaw real que a gente esta tentando obter
		 g.DrawLine(penTracejada, 128, 128, tx, ty);

		 
		}
	 }

	 // Atualiza a PictureBox
	 pb_map.Image = bmp;
	}

	private void button8_Click(object sender, EventArgs e)
	{
	 lb_log.Text = ""; // limpa
	}
	// BOTAO COMBAT ONLY
	
	private void button12_Click(object sender, EventArgs e)
	{
	 loga("Modo combate estático ativado.");
	 on = true;
	 cb_anda.Checked = false;
	 on = true;
	 while (on)
	 {
		checkme();
		moveto(me.pos);
		wait(200);
	 }

	 
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
		 File.WriteAllText("discord.cfg", tb_filename.Text.Trim());

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
		 File.WriteAllText("discord.cfg", nomeSimples); // salva nome no cfg

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
