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
	 private const int max_count = 15;

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



	// CLASSE DECAYSESSION - CALCULA O DECAY DE VIDA DO PLAYER
	public class DecaySession
	{
	 private int hp_ini;
	 private int hp_ant;
	 private long t_ini;
	 private long t_ant;
	 private int current;

	 public void Start(int hp)
	 {
		hp_ini = hp;
		hp_ant = hp;
		t_ini = Environment.TickCount;
		t_ant = t_ini;
		current = 0;
	 }

	 public void Update(int hp)
	 {
		long now = Environment.TickCount;
		int dhp = hp_ant - hp;
		int dms = (int)(now - t_ant);

		if (dhp > 0 && dms > 0)
		 current = (int)(dhp * 60000 / dms); // hp por minuto

		hp_ant = hp;
		t_ant = now;
	 }

	 public int Current => current;

	 public int End(int hp_final)
	 {
		int total_loss = hp_ini - hp_final;
		int total_time = (int)(Environment.TickCount - t_ini);

		if (total_time <= 0 || total_loss <= 0) return 0;

		return (int)(total_loss * 60000 / total_time); // hp por minuto -  decay médio da luta
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