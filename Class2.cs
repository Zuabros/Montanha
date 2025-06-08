using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Discord.Form1;

namespace Discord
{
 // --------------------------------
 // CLASSE FUNCOES - MÉTODOS AUXILIARES
 // --------------------------------

 public static class funcoes
 {
	// --------------------------------
	// M18 - MÉTODO ISPRATO - VERIFICA SE A COR É DO PRATO (MARCADOR)
	// --------------------------------
	public static bool isprato(Color c)
	{
	 return Math.Abs(c.R - 230) <= 3 &&  // tolerância de 3 para o canal vermelho (esperado: 230)
					Math.Abs(c.G - 240) <= 3 &&  // tolerância de 3 para o canal verde   (esperado: 240)
					Math.Abs(c.B - 250) <= 3;    // tolerância de 3 para o canal azul    (esperado: 250)
	}
	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
	private const int MOUSEEVENTF_LEFTDOWN = 0x02;
	private const int MOUSEEVENTF_LEFTUP = 0x04;

	// Constantes de mouse e teclado

	public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
	public const int MOUSEEVENTF_RIGHTUP = 0x10;

	// M07 - MÉTODO DOMOUSECLICK - REALIZA UM CLIQUE DO MOUSE.
	public static void DoMouseClick(int botao = 1)
	{
	 int X = Cursor.Position.X;
	 int Y = Cursor.Position.Y;
	 if (botao == 1)
		mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
	 else if (botao == 2)
		mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
	}
	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(IntPtr hWnd); // traz janela pro foco

	// M04 - MÉTODO FOCAR WOW - TRAZ A JANELA DO WOW PARA FRENTE.
	public static void focawow()
	{
	 Process[] prc = Process.GetProcessesByName("WowClassic"); // Busca processo do WoW Classic
	 if (prc.Length > 0) // Verifica se o processo foi encontrado
	 {
		SetForegroundWindow(prc[0].MainWindowHandle); // Ativa a janela do WoW
	 }
	 else
	 {
		MessageBox.Show("Janela do WOW Classic não localizada. Encerrando aplicativo."); // Alerta
		Application.Exit(); // Encerra o app
	 }
	}

	// M06 - MÉTODO MOUSEMOVE - MOVE O CURSOR PARA A POSIÇÃO (X, Y).
	public static void mousemove(int x, int y)
	{
	 Cursor.Position = new Point(x, y);
	}
	// CLASSE DECAYTRACKER - MONITORA O DECAY DE VIDA DO PLAYER
	public  class DecayTracker
	{
	 private Queue<int> history = new Queue<int>();
	 private const int max_count = 12;

	 public int current { get; private set; } = 0;

	 public void Add(int value)
	 {
		current = value;
		history.Enqueue(value);
		if (history.Count > max_count)
		 history.Dequeue();
	 }

	 public int Average
	 {
		get
		{
		 if (history.Count == 0) return 0;

		 List<int> list = history.ToList();
		 list.Sort();
		 int n = list.Count;

		 if (n % 2 == 1)
			return list[n / 2];
		 else
			return (list[n / 2 - 1] + list[n / 2]) / 2;
		}
	 }
	}



	// ----------------------------------------------
	// CLASSE DECAYSESSION - SLIDING WINDOW 10 SEG
	// ----------------------------------------------
	public class DecaySession
	{
	 // struct interna para guardar eventos de dano
	 private struct evt
	 {
		public long t;    // timestamp em ms
		public int dmg;   // valor do dano
	 }

	 private Queue<evt> fila; // fila dos eventos recentes
	 private int hp_ini;      // hp inicial da luta
	 private int hp_ant;      // hp do último tick
	 private long t_ini;      // timestamp do início
	 private long t_ant;      // timestamp do último tick

	 public DecaySession() // construtor
	 {
		fila = new Queue<evt>(); // inicia fila
	 }

	 public void Start(int hp) // inicia nova sessão
	 {
		hp_ini = hp;                 // guarda HP inicial
		hp_ant = hp;                 // define último HP
		t_ini = Environment.TickCount; // timestamp inicial
		t_ant = t_ini;               // último tick = início
		fila.Clear();                // limpa eventos anteriores
	 }

	 public void Update(int hp) // chamada periódica
	 {
		long now = Environment.TickCount; // tempo atual
		int dhp = hp_ant - hp;            // calcula perda de HP desde o último tick

		if (dhp > 0) // só registra se houve dano
		{
		 evt e;
		 e.t = now;  // tempo do evento
		 e.dmg = dhp; // valor do dano
		 fila.Enqueue(e); // adiciona à fila
		}

		hp_ant = hp; // atualiza o último HP
		t_ant = now; // atualiza o último tempo
	 }

	 public int Current(int avg_decay) // decay atual (com média antes dos 10s)
	 {
		int segundos_decay = 8 * 1000; // 8 segundos em milissegundos
		long agora = Environment.TickCount; // tempo atual

		if (agora - t_ini < segundos_decay) // ainda nos primeiros 10s de luta
		 return avg_decay; // retorna média anterior

		// limpa eventos com mais de 10s
		while (fila.Count > 0 && agora - fila.Peek().t >	segundos_decay)
		 fila.Dequeue();

		if (fila.Count == 0) return 0; // nenhum evento recente

		int total = 0; // soma dos danos
		long mais_velho = fila.Peek().t;
		long mais_novo = fila.Last().t;
		long dur = mais_novo - mais_velho;

		foreach (var e in fila)
		 total += e.dmg;

		if (dur <= 0) dur = 1; // evita divisão por zero

		return (int)(total * 60000 / dur); // hp por minuto
	 }

	 public int End(int hp_final) // decay médio da luta
	 {
		int total_loss = hp_ini - hp_final;
		int total_time = (int)(Environment.TickCount - t_ini);

		if (total_time <= 0 || total_loss <= 0) return 0;

		return (int)(total_loss * 60000 / total_time); // hp por minuto
	 }
	}






	// --------------------------------
	// FUNÇÃO MAPEIA
	// Marca uma posição no vetor lógico do mapa com determinado código
	// player é obrigatório
	// --------------------------------
	public static void mapeia(byte[,] map, loc pos, byte code, element player)
	{
	 int dx = (int)(pos.x - player.pos.x); // deslocamento em X
	 int dy = (int)(pos.y - player.pos.y); // deslocamento em Y

	 int mx = 128 + dx; // coordenada lógica X no vetor
	 int my = 128 + dy; // coordenada lógica Y no vetor

	 if (mx >= 0 && mx < 256 && my >= 0 && my < 256)
		map[mx, my] = code; // aplica o código no vetor lógico
	}

	// O MOB É GRAY?
	public static bool isgray(int me, int mob)
	{
	 if (me < 6) return false;
	 if (me == 6 && mob == 1) return true;
	 if (me >= 7 && me <= 19 && mob <= me - 6) return true;
	 if (me >= 20 && me <= 29 && mob <= me - 7) return true;
	 if (me >= 30 && me <= 39 && mob <= me - 8) return true;

	 if (me >= 40 && me <= 44 && mob <= me - 9) return true;
	 if (me >= 45 && me <= 49 && mob <= me - 10) return true;
	 if (me >= 50 && me <= 54 && mob <= me - 11) return true;
	 if (me >= 55 && me <= 59 && mob <= me - 12) return true;
	 if (me >= 60 && mob <= me - 13) return true;
	 return false;
	}
	// ---------------------------------------------------------
	// MÉTODO isdown - Retorna true se a tecla estiver pressionada
	// ---------------------------------------------------------
	[System.Runtime.InteropServices.DllImport("user32.dll")]
	static extern short GetAsyncKeyState(int vKey);
	public static bool isdown(byte key)
	{
	 return (GetAsyncKeyState(key) & 0x8000) != 0;
	}







	// M02 - MÉTODO GETCOLORAT - CAPTURA A COR DO PIXEL NA COORDENADA (X, Y).
	public static Color GetColorAt(int x, int y)
	{
	 using (Bitmap bmp = new Bitmap(1, 1)) // Cria bitmap de 1x1 pixel para captura
	 {
		using (Graphics g = Graphics.FromImage(bmp)) // Cria contexto gráfico para o bitmap
		{
		 g.CopyFromScreen(x, y, 0, 0, new Size(1, 1)); // Copia pixel da tela para o bitmap
		}
		return bmp.GetPixel(0, 0); // Retorna a cor do pixel capturado
	 }
	}
 }
}