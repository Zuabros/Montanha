using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices; //required for APIs
using System.Diagnostics;
using System.IO;
using System.Threading;
//using Emgu;
//using Emgu.CV;
//using Emgu.CV.Structure;
//using Emgu.CV.CvEnum;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;



namespace Amigão
{
    public partial class Form1 : Form
    {
        // ---------------------
        // AUTOEXEC CODE
        //----------------------
        public Form1()
        {
            InitializeComponent();
            // this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            // MessageBox.Show("rodei");

        }


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
        /// <summary>Must initialize cbSize</summary>
        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(ref CURSORINFO pci);

        private const Int32 CURSOR_SHOWING = 0x00000001;
        private const Int32 CURSOR_SUPPRESSED = 0x00000002;



        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int PLAYERSTATSX = 1574; // pixel do status do player no addon
        public const int PLAYERSTATSY = 470; // pixel do status do player no addon 

        public const int PLAYERLOCXX = 1591; // pixel da coordenada X + SPEED
        public const int PLAYERLOCXY = 517; //  
        public const int PLAYERLOCYX = 1645; // pixel a coordenada Y + FACING 
        public const int PLAYERLOCYY = 517; //  

        public const int TARGETX = 1623; // quadrado de hp do mob 
        public const int TARGETY = 332; //  

        public const int MEX = 1626; // meu quadrado de vida / hp 
        public const int MEY = 280; // 

        public const int TURNX = 1575; // meu quadrado de vida / hp 
        public const int TURNY = 608; // 

        public const int HBX = 1775; // meu quadrado de vida / hp 
        public const int HBY = 344; // 

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int QKEY = 0x51; // tecla Q 
        public const int WKEY = 0x57; // tecla W 
        public const int EKEY = 0x45; // tecla E 
        public const int SKEY = 0x53; // tecla S 
        public const int IKEY = 0x49; // tecla I 
        public const int AKEY = 0x41; // tecla A 
        public const int DKEY = 0x44; // tecla D 
        public const int SPACEBAR = 0x20; // espaço
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
        public const int N1 = 0x61; // tecla 1 
        public const int N2 = 0x62; // tecla 2 
        public const int N3 = 0x63; // tecla 3 
        public const int N4 = 0x64; // tecla 4 
        public const int N5 = 0x65; // tecla 5 
        public const int N6 = 0x66; // tecla 6 
        public const int N7 = 0x67; // tecla 7 
        public const int N8 = 0x68; // tecla 8 
        public const int N9 = 0x69; // tecla 9 
        public const int N0 = 0x60; // tecla 0 
        public const int ENTER = 0x0D; // tecla ENTER 
        public const int BARRA = 0x6E; // tecla BARRA 
        public const int F1 = 0x70; // tecla F1
        public const int F2 = 0x71; // tecla F2 
        public const int F3 = 0x72; // tecla F3 
        public const int F4 = 0x73; // tecla F4 
        public const int F5 = 0x74; // tecla F5 
        public const int F6 = 0x75; // tecla F6 
        public const int F7 = 0x76; // tecla F7 
        public const int F8 = 0x77; // tecla F8 
        public const int ELEMENTAL = 0; // family elemental
        public const int MECHANICAL = 26; // family MECHANICAL
        public const int UNDEAD = 50; // family UNDEAD
        public const int HUMANOID = 78; // humanoid
        public const int BEAST = 100; // family elemental
        public const int GIANT = 128; // family giant
        public const int ELITE = 255; // ELITE MOB


        //----------------------------------
        // FUNÇÕES PADRÃO - REPETEM SEMPRE
        //---------------------------------
        // Função focawow(), Foca janela do wow


        public IntPtr getcursor() // 
        {

            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(ref pci);
            rtbcursor.Text += pci.hCursor.ToString() + " ";
            return pci.hCursor;
        }
        public static void focawow()
        {
            var prc = Process.GetProcessesByName("wowclassic");
            if (prc.Length > 0)
            {
                SetForegroundWindow(prc[0].MainWindowHandle);
            }
            else
                MessageBox.Show("Wow window not found");

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
        public static void clica(int x, int y, int botao = 1)
        {
            focawow();
            mousemove(x, y);
            DoMouseClick(botao);
            if (botao == 1) wait(1501);
            if (botao > 50) wait(botao);
        }
        // Espera milisegundos sem travar a interface 
        public static void wait(int milliseconds)
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds <= milliseconds)
            {
                
                Application.DoEvents();
            }
        }
        // ~Retorna a cor de um pixel 
        public static Color vecor(int x, int y)
        {
            Bitmap bmp = new Bitmap(1, 1);
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp.GetPixel(0, 0);
        }
        //----------------------------------
        // FUNÇÕES ESPECIFICAS
        //---------------------------------
        // FUNÇÕES PALADINO
        //----------------------------------
        // CHECK SEAL OF RIGHTEOUSNESS
        bool sor()
        {
            return !veiscor(1627, 391, 256, 0, 0);
        }
        // CHECK BLESSING OF MIGHT
        bool might()
        {
            return !veiscor(1650, 387, 256, 0, 0);
        }
        // CHECK DEVOTION AURA
        bool devo()
        {
            return !veiscor(1185, 958, 151, 139, 151);
        }
        // CHECK FIRE RESISTANCE AURA AURA
        bool fireaura()
        {
            return !veiscor(1353, 958, 239, 195, 0);
        }
        // CHECK RETRIBUTION AURA
        bool retri()
        {
            return !veiscor(1229, 951, 104, 86, 112);
        }
        void checkaura()
        {
            if (rbretriaura.Checked)
            { if (!retri()) aperta(N3); }// Retribution aura 
            else if (rbdevoaura.Checked)
            { if (!devo()) aperta(NOVE); } // Devotion Aura 
            else if (rbfireaura.Checked)
            { if (!fireaura()) aperta(F6); }// Fire Aura
        }

        // VE SE NAO TA DE COSTAS
        //---------------------------------
        public static void checkface()
        {

            Color tcor = vecor(TURNX, TURNY);
            if (!rot && tcor.R > 200 && tcor.G < 100 & tcor.B < 50)
            {
                
                aperta(SPACEBAR);
                if (!me.meleerange) aperta(QKEY, 1000);
                aperta(IKEY, 200);
                aperta(WKEY);

                //if (me.mana < 10) aperta(EKEY, 400);
            }
        }

        //----------------------------------
        // APERTA TECLA 
        //---------------------------------
        public static void aperta(byte key, int time = 20) // 2 apenas solta a tecla
        {
            focawow();
            if (time != 2) keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0);
            if (time != 2) wait(time); // 2 = apenas solta a tecla
            if (time > 0) keybd_event(key, 0, KEYEVENTF_KEYUP, 0); // solta a tecla
        }

        //----------------------------------
        // CASTA SKILL 
        //---------------------------------
        public static void cast(byte tecla, int dur = 1600) // global cooldown 1.5s
        {
            //aperta(ZERO); // stopcasting
            //wait(200);
            aperta(tecla); // casta spell do livro 
            wait(300);
            checkface();
            wait(dur - 300);

        }
        //----------------------------------
        // CASTA SKILL 
        //---------------------------------
        void kast(byte tecla, int dur = 1600) // global cooldown 1.5s
        {
            //aperta(ZERO); // stopcasting
            //wait(200);
            checkme();
            if (me.combat)
            {
                cast(tecla, dur);
            }

        }



        //----------------------------------
        // VIRA O CHAR PARA COORDENADA X,Y
        //---------------------------------
        public void gira(loc tar)
        {
            // Pega o angulo a virar
            double ang = Math.Atan2(me.pos.x - tar.x, me.pos.y - tar.y) / Math.PI;
            double yaw;
            if (ang < 0) ang += 2;
            double pitch = Math.Round(ang * 1000);
            if (pitch > me.face) yaw = pitch - me.face;
            else if (pitch < me.face) yaw = 2000 - me.face + pitch;
            else yaw = 0;
            if (debug)
            {

                tbyaw.Text = yaw.ToString();
                tbpitch.Text = pitch.ToString();
            }
            double pausa;
            if (yaw > 1000) pausa = 2000 - yaw; else pausa = yaw;
            if (me.spd > 10) pausa *= 1.5; // vira mais se estiver andando
            if (pausa > 100)
            {
                aperta(SPACEBAR); // jump to create realism
                if (yaw > 1000) aperta(EKEY, (int)pausa); // viara para direita 
                else aperta(QKEY, (int)pausa); // vira para esquerda 
            }

        }
        bool jarecuou = false;
        public void combatloop()
        {
            
            loga("Starting Combat Loop " + combatcount.ToString()); ; combatcount++;

            int counter = 0;
            aperta(WKEY, 2); // para de andar 
            bool needloot = true;
            bool temrenew = false;
            bool jacurei = false;
            bool urso = false;
            Color tempcor;
            int inihp = me.hp;
            int inimana = me.mana;
            bool bsk = false;
            if (rbbersek.Checked) bsk = true;
            bool vaicurar = false; // PALADIN
            while (me.combat)
            {
                if (!rot) aperta(WKEY, 2); // para de andar
                if (me.morreu) break;
                tempcor = vecor(1636, 335);
                ghostwpdone = false;
                //----------------------------------
                // DRUID COMBAT ROUTINE - DCR
                //-----------------------------------
                if (rbdruid.Checked) // rotina para druida 
                {

                    bool deugrito = false;
                    if ((me.hp > 90 && me.trivial) || me.mana < 50 && !urso) // se vida cheia e for trivial 
                    {
                        cast(NOVE); // forma de urso 
                        urso = true;
                    }
                    else if (!urso) // vida baixa ou mob não trivial 
                    {

                        cast(OITO); cast(OITO);
                        urso = true;
                        aperta(IKEY);
                        //aperta(SEIS);
                    }

                    checkme();
                    checkface();

                    if (me.mana >= 10)
                    {
                        if (!deugrito) { cast(UM); deugrito = true; }
                        else if (deugrito) cast(DOIS);
                    }
                    if (!me.meleerange && me.tarhp < 100) cast(IKEY); // sem range

                    if (me.tarhp == 100 && !me.meleerange)
                    {
                        if (me.hp < 100) cast(IKEY); // interage com alvo. 
                        else if (me.hp == 100) cast(SEIS);
                    }

                    if (me.hp < 50) //ta morrendo
                    {
                        if (!temrenew)
                        {
                            cast(OITO);
                            checkme();
                            if (me.mana < 40) jacurei = true;
                            cast(OITO);
                            aperta(SEIS);
                            temrenew = true;
                        }
                        if (false && me.hp < 30 && temrenew && !jacurei)
                        {
                            //cast(TRES,2400); cast(TRES); // renew, bear form 
                            //aperta(SEIS); // clear target
                            cast(OITO);
                            cast(OITO);
                            aperta(SEIS);
                            jacurei = true;
                        }
                    }

                    if (me.tarhp == 0 || vecor(TARGETX, TARGETY) == Color.Black) cast(SEIS); // perdeu target
                    //getcursor();
                    aperta(QUATRO);
                    //aperta(IKEY);
                    if (me.tarhp < 0) cast(IKEY, 1000);
                }
                //----------------------------------
                // WARLOCK COMBAT ROUTINE - WCR
                //-----------------------------------
                // 
                else if (rblock.Checked) // rotina para warlock 
                {
                    if (me.hp < 20) { aperta(OITO); }// TOMA POÇÃO
                    if (me.hp < 10 && me.level >= 16 && tempet) clica(950, 914); // SACRIFICA VOID
                    checkme(); // verifica status do mob e meuy
                    if (me.tarhp == 0 || vecor(TARGETX, TARGETY) == Color.Black) kast(SEIS); // perdeu target
                    if (me.hp < 60 && veiscor(860, 992, 255, 255, 110)) { aperta(N6); } // usa HS
                    checkme();
                    if (!veiscor(179, 135, 256, 134, 256)) // pet morrendo 
                        {
                        if (me.hp > 60 && me.combat) cast(N3, 4000); // health funnel em combate
                        if (!me.combat && me.level >= 18) { clica(994, 911); wait(2000); }// consume shadows
                        }
                    
                    if (false) //me.meleerange && veiscor(778, 1045, 255, 26, 26)) // fora de range do drain life
                    {
                        kast(N3, 300); // vira para o pet (arrumarr a direção)
                        kast(SEIS); // limpa target, para de atacar 
                        aperta(WKEY); // anda 
                        Thread.Sleep(1000); // espera um segundo 
                    }
                    
                    if (me.hp < 50 && !veiscor(860, 992, 255, 255, 110)) // vida baixa, pedra no cooldown
                    {
                        if (!veiscor(778, 1045, 255, 26, 26)) kast(N5, 4000); // range do drain life ?
                        else { cast(WKEY); } // anda pra frente 
                    }
                    if (me.mana < 15) kast(SETE, 6000); // sem mana, wand 
                    // DOTs 
                    else if (me.tarhp > 35 && !weak)
                    {
                        if (!me.tarcaster) kast(DOIS); // acima de 35% vida, weaknes
                        else cast(N2);
                    }
                    else if (me.tarhp > 40 && !immo) kast(UM, 2100); // acima de 50% vida, immolate
                    checkme();
                    
                    if (me.tarhp > 5 && !corr) kast(TRES); // acima de 20% vida, corruption
                    // ataques diretos 
                    else if (me.tarhp < 20 & !drain) kast(N8); // drain soul abaixo de 20% 
                    else
                    {
                        if (me.hp < 70 && !veiscor(860, 992, 255, 255, 110)
                                && !me.lifeless & corr && weak) kast(QUATRO, 3000); // drain life 
                        //---------------------
                        // APPLY FILLER 
                        //---------------------
                        else if(corr && weak) // filler only if maain buffs applied
                        {
                            checkme(); 
                            if (!cbwand.Checked && !drain) kast(N8); // Drain Soul Filler 
                            else  kast(SETE); // Wand Filler
                        }
                    }
                    // Corrige bugs 
                    if (me.tarhp == 100 && !me.meleerange) cast(SEIS); // tira o target se out
                    checkface();
                    //wait();
                }
                //----------------------------------
                // HUNTER COMBAT ROUTINE - HCR
                //-----------------------------------
                else if (rbhunta.Checked)
                {
                    counter++;
                    checkme();
                    if (me.mana >= 15)
                    {
                        kast(TRES);
                    }
                    if (!me.meleerange && me.tarhp < 100) kast(IKEY); // sem range

                    if (me.tarhp == 100 && !me.meleerange)
                    {
                        if (me.hp < 100) kast(IKEY); // interage com alvo. 
                        else if (me.hp == 100 && me.combat) kast(SEIS);
                    }
                    
                    if (!me.rangedrange)
                    {
                        if (!urso) { kast(IKEY);urso = true; }
                        else if (counter % 7 == 0) kast(IKEY);
                    }

                }
                //----------------------------------
                // WARRIOR COMBAT ROUTINE - WACR
                //-----------------------------------
                
                else if (rbwarr.Checked && !me.morreu)
                {
                    
                    checkme();
                    if (me.hp > 85 && rbnormal.Checked) { bsk = true; };
                    alternator = !alternator; 
                    if (alternator && !rot) aperta(WKEY, 2);
                    if (me.mobcount >= 2) bsk = false;
                    if (me.mobcount > 1 && me.hp < 100 && !me.tarcaster && me.meleerange) 
                    {
                        if (!rot) cast(SKEY, 800); // recua 2 segundos se mais de 1 mob
                    }
                    else if (me.tarcaster) wait(400);
                    //if (me.tarcaster) aperta(SPACEBAR);
                    //----------------------------------
                    // GENERAL ROUTINE TO MANAGE TARGETS
                    checkme();
                    if (!me.tarcombat && me.combat) // target not in combat (wrong target) 
                    {
                        aperta(SEIS); // clear target 
                        if (drot) aperta(F6); // assist tank 

                    }
                    else if (me.tarcombat && me.combat && me.tarhp != 0) // focus dead 
                    {
                        if (!rot) aperta(IKEY); // Focus target 
                        //else if (drot) aperta(F6); // assist tank 
                    }
                    if (!me.meleerange || me.tarhp==0) aperta(F6);
                    //------------------------------------------

                    // CHECK AND CRY BATTLE SHOUT
                    if (vecor(1799, 303).G > 200 && me.mana >= 10) aperta(NOVE); // BATTLE SHOUT
                    Color charge = vecor(687, 1061);
                    // OVERPOWER WHEN PROC 
                    if (!veiscor(817, 1053, 23, 27, 30) && me.mana >= 5) aperta(CINCO); // overpower
                    //EXECUTE - SEM TALENTO = 15 RAGE
                    else if (me.tarhp <= 20 && me.mana>= 13) aperta(N8);
                    //SUNDER ARMOR
                    else if (!drot && me.tarhp > 70 && me.mana >= 15) kast(N5); // sunder armor
                    //BLOODRAGE
                    else if (me.tarhp > 70 && veiscor(861, 1005, 236, 84, 33) && me.mana < 15) aperta(N6); // 
                    // CLEAVE WHEN MORE THAN ONE MOB
                    else if (drot && me.mobcount>1)
                    {
                        if ((me.mana >=20) // enough mana
                        && (!veiscor(1031,988,178,152,53)) // NOT ALREADY CLEAVE CASTED
                        &&  (veiscor(1020,1006,107,105,107))) // CLEAVE UP
                        {
                            aperta(N9); // cast cleave
                        }
                    }
                    // DUMP RAGE (HEROIC STRIKE) - rage > 40
                    else if ((me.mana > 25 || (me.mana > 15 && bsk)) && !iscor(charge, 255, 178, 67) && me.tarhp > 20) // ja ativado heroic strike 
                    {
                        if (iscor(charge, 123, 16, 1)) aperta(DOIS); // HS up, use HS
                    }
                    // CHECK AND APPLY REND (tarhp>= 30)
                    else if (tbuserend.Checked &&  !drot && !me.lifeless 
                        && !rend && me.mana >= 10 && me.tarhp > 30) kast(TRES); // rend

                    // THUNDERCLAP BELOW 70%  
                    checkme();
                    if (!me.meleerange && !rot) aperta(IKEY);
                    if (usethun.Checked && !bsk&& !thun && me.mana >= 20 && (me.hp < 70||
                        (me.tarlevel>=me.level && !me.tarcaster))) aperta(QUATRO); // thunderclap
                    else if(drot && me.mobcount >1 && !thun && me.mana >=20) aperta(QUATRO); // dungeon tclap

                    // STONEFORM BELOW 65%
                    if (me.hp < 65 && !veiscor(962, 1041, 50, 48, 31)) aperta(OITO);
                    checkme();
                    // POTION BELOW 10%
                    if (me.hp < 15) aperta(SETE);
                                        
                    // CHECK AND APPLY DEMORALIZING SHOUT (tarhp>= 20)
                    if (!drot && me.mobcount > 1 && !bsk && (me.hp < 80||(me.tarlevel >= me.level&&!me.tarcaster)) && !me.tarcaster
                        && !demo && me.mana >= 10 && me.tarhp > 25) aperta(N2); // DEMO SHOUT

                    // CHECK AND APPLY HAMSTRING (humanoid below 50%)
                    if (rbhamstring.Checked && me.humanoid && me.tarhp < 35 && !hams && me.mana >= 10) aperta(N3); // hamstring, avoid flee

                    // RETALIATION IF BELOW 35% HP
                    if (!rot && !me.tarcaster&& (me.hp < 35 || (me.mobcount >=3 && me.hp <65)) && veiscor(757,989,218,216,209)) aperta(N4); // 

                    // DEATH WISH OR BLOODRAGE AT HIGH LEVEL MOBS OR BERSEK
                    if ((me.tarlevel >= me.level + 1)||bsk)
                        {
                         
                        if (veiscor(861, 1005, 236, 84, 33) && me.tarhp > 35) aperta(N6); // blood rage
                        if (veiscor(560, 1040, 87, 20, 21) && me.mana > 10 && me.tarhp > 60) aperta(F7); //Death wish

                        }
                }
                //----------------------------------
                // PALADIN COMBAT ROUTINE - 

                //-----------------------------------
                // Bersek: Ret aura, margem segurança 15%
                // Normal: Devotion aura, margem segurança 20%
                // Cautious: Ret aura, margem segurança 25%
                else if (rbpala.Checked)
                {
                    counter++;

                    //----------------------------------
                    // check auras and buffs 
                    checkaura();



                    // CHECK AND CAST BUFFS
                    if (!sor() && me.mana >= 8) aperta(DOIS); // SOR
                    if (!might() && me.mana >= 25) aperta(QUATRO); // SOR
                    //----------------------------------
                    // GENERAL ROUTINE TO MANAGE TARGETS
                    checkme();
                    if (!me.tarcombat && me.combat) // target not in combat (wrong target) 
                    {
                        loga("Combat: no valid target");
                        aperta(SEIS); // clear target 
                        if (drot) aperta(F6); // assist tank 

                    }
                    else if (me.tarcombat && me.combat && me.tarhp != 0) // focus  
                    {
                        if (!rot) aperta(IKEY); // Focus target 
                        //else if (drot) aperta(F6); // assist tank 
                    }
                    //if (!me.meleerange || me.tarhp == 0) aperta(F6);

                    //------------------------------------------
                    // SKILL CASTING (NOT MUCH)
                    //------------------------------------------
                    // HEAL IF NEEDED (BUBBLE-HEAL)
                    //--------------------------------------------
                    int lowhp = Convert.ToInt32(tbfoodhp.Text) / 2;
                    /*
                    if (rbnormal.Checked) lowhp = healat/2;
                    else if (rbbersek.Checked) lowhp = healat - 30;
                    else if (rbcautious.Checked) lowhp = healat - 30;
                    */
                    if (me.hp <= lowhp*2 && veiscor(841, 993, 248, 251, 246)) aperta(N6); // trinket 1
                    if (me.hp <lowhp && me.mana > 20 && me.combat)
                    {
                        bool forb = true;
                        // learn at level 10 only blessing of protection 
                        if (me.level < 10 || veiscor(962, 1002, 169, 170, 168) &&
                           ( veiscor(1011, 1002, 95, 22, 13))) forb = false; // BUBBLE
                        if (!forb) // not forbearance 
                        {
                            if (!veiscor(991, 992, 50, 34, 18)) // divine protection up 
                            {
                                vaicurar = true;
                                loga("Combat: Divine protection");
                                kast(N9);
                                
                            }
                            else if (!veiscor(941, 985, 48, 50, 51)) // BOP UP 
                            {
                                vaicurar = true;
                                loga("Combat: Blessing of Protection");
                                kast(N8);
                            }
                            if (vaicurar && veiscor(1646, 196, 171, 158, 120))
                            {
                                kast(OITO,2500); // holy light 
                                loga("Combat: Holy Light (1)");
                                if (me.hp < 95)
                                {
                                    kast(OITO, 2500); // holy light 
                                    loga("Combat: Holy Light (2)");
                                }
                                vaicurar = false;
                            }

                        }
                        
                    }
                    // LAY ON HANDS 
                    if (me.hp < 7) aperta(N5);
                    // POTION
                    else if (me.hp < 12) aperta(CINCO); // potion
                    // STONEFORM ON 60% HP
                    if (me.hp < 60 && veiscor(892, 1049, 170, 171, 152)) aperta(SETE);
                    // HAMMER OF WRATH
                    if (cbhow.Checked && veiscor(889,993,64,87,80) ) aperta(N7);
                    // INTERRUPT CAST WITH HOJ
                    else if (me.rangedrange && me.mana > 20 && veiscor(343, 181, 157, 109, 0) &&
                        me.tarhp>30 && veiscor(690, 1036, 229, 172, 113)) aperta(TRES);
                    // EXORCISM IF UNDEAD
                    else if (me.level >= 21 && me.type==UNDEAD && me.mana>30 && veiscor(737,988,255,254,6)) aperta(N4);
                    // JUDGEMENT ON COOLDOWN
                    else if (veiscor(592, 993, 222, 215, 158)&&me.mana >=15 ) aperta(N2); // JUDGEMENT
                    if (me.tarhp > 35 && me.combat && counter % 7 == 0) aperta(SPACEBAR);
                    wait(400);
                }
                
                
            }
            checkme();
            
            // AUTO ADJUST FOOD EAT TRESHOLD
            if (cbauto.Checked && !me.morreu)
            {
                
                int margin=0;
                if (rbbersek.Checked) margin = 15;
                else if (rbnormal.Checked) margin = 20;
                else if (rbcautious.Checked) margin = 25;
                
                // ADJUST FOOD / HEAL TRESHHOLD
                int damage = inihp - me.hp;
                int current = Convert.ToInt32(tbfoodhp.Text);
                if (damage + margin >= 80) { damage = 80; margin = 0; }
                if (current < damage+margin) tbfoodhp.Text = (damage+margin).ToString(); // sobe 
                if (current > damage + margin)
                {
                    alternator2 = !alternator2; // every 2 steps 
                    if (alternator2) tbfoodhp.Text = (current - 1).ToString(); // desce
                }

                // ADJUST MANA / TRESHHOLD
                damage = inimana - me.mana;
                margin -= 10;
                current = Convert.ToInt32(tbmanahp.Text);
                if (current < 0) current = 0;
                if (damage + margin >= 70) { damage = 70; margin = 0; }
                if (current < damage + margin)
                          tbmanahp.Text = (damage + margin).ToString(); // sobe 
                if (current > damage + margin && current != 0)
                {
                    alternator2 = !alternator2; // every 2 steps 
                    if (alternator2) tbmanahp.Text = (current - 1).ToString(); // desce
                }


            }

            jarecuou = false;
            if (!me.morreu && needloot & !me.combat)
            {
                //if (me.hp < 15) aperta(F3);
                if (!drot) loot();
            }
            /*
            tempcor = vecor(902, 247);
            if (tempcor.R > 100 && tempcor.G < 20 && tempcor.B < 20) // morreu 
            {
                morreu = true;
                clica(902, 247);
                wait(2000);
                MessageBox.Show("Break point.");
             
             }
             */
        }
        bool alternator = false;
        bool alternator2 = false;
        int combatcount = 1;
        bool iscor(Color tcor, int r, int g, int b)
        {
            bool redok = false;
            bool greenok = false;
            bool blueok = false;
            if (tcor.R == r || r == 256) redok = true;
            if (tcor.G == g || g == 256) greenok = true;
            if (tcor.B == b || b == 256) blueok = true;
            if (redok && greenok && blueok) return true;
            return false;
        }
        bool veiscor(int x, int y, int r, int g, int b)
        {
            Color tcor = vecor(x, y);
            if (iscor(tcor, r, g, b)) return true;
            return false;
        }
        void leslot(Color tcolor, int slot)
        {
            if (rblock.Checked)
            {
                if (isimmo(tcolor, slot)) { immo = true; cbimmo.Checked = true; }
                if (isweak(tcolor, slot)) { weak = true; cbcow.Checked = true; }
                if (iscorr(tcolor, slot)) { corr = true; cbcorr.Checked = true; }
                if (isdrain(tcolor, slot)) { drain = true; cbdrain.Checked = true; }
            }
            else if (rbdruid.Checked)
            {
                if (isfire(tcolor, slot)) { fire = true; cbfire.Checked = true; }
            }
            else if (rbwarr.Checked )
            { 
           // check for warrior debuff icons: rend, thunderclap, demo shout, hamstring
                if (iscor(tcolor, 0,0,0)) { rend = true; cbrend.Checked = true; }
                if (iscor(tcolor, 0,48,90)) { thun = true; cbthun.Checked = true; }
                if (iscor(tcolor, 0,60,0)) { demo = true; cbdemo.Checked = true; }
                if (iscor(tcolor, 5,7,8)) { hams = true; cbhams.Checked = true; }

            }
        }
        void vedebuffs()
        {
            int inix = 324; int iniy = 116;
            int startx = 346;
            if (rblock.Checked)
            {
                cbimmod.Checked = cbcorrd.Checked = cbcowd.Checked = cbdraind.Checked = false;
                immo = false; cbimmo.Checked = false; // reseta status de debuffs 
                corr = false; cbcorr.Checked = false; // corruption 
                weak = false; cbcow.Checked = false; // curse of weakness
                drain = false; cbdrain.Checked = false; // curse of weakness

            }
            else if (rbdruid.Checked)
            {
                cbfired.Checked = false;
                fire = false; cbfire.Checked = false;
            }
            else if (rbwarr.Checked)
            {
                rend = false; cbrend.Checked = false; // reseta status de debuffs 
                thun = false; cbthun.Checked = false; // reseta status de debuffs 
                demo = false; cbdemo.Checked = false; // reseta status de debuffs 
                hams = false; cbhams.Checked = false; // reseta status de debuffs 
            }

            int add = 99;

            Color tcolor = vecor(inix, iniy);
            leslot(tcolor, 1); // verifica primeiro slot de debuff

            add = 29;
            //tbadd.Text = add.ToString();
            inix += add;
            startx += add; // 
            tcolor = vecor(inix + 1, iniy);
            leslot(tcolor, 2); // verifica segundo slot de debuff


            add = 30;
            tbadd2.Text = add.ToString();
            inix += add;
            startx += add; // 
            tcolor = vecor(inix, iniy);
            leslot(tcolor, 3); // verifica terceiro slot de debuff

            add = 30;
            tbadd2.Text = add.ToString();
            inix += add;
            startx += add; // 
            tcolor = vecor(inix, iniy);
            leslot(tcolor, 4); // verifica quarto slot de debuff





            if (debug)
            {
                if (rblock.Checked)
                {
                    if (immo) cbimmod.Checked = true;
                    if (corr) cbcorrd.Checked = true;
                    if (weak) cbcowd.Checked = true;
                    if (drain) cbdraind.Checked = true;
                }
                else if (rbdruid.Checked)
                {
                    if (fire) cbfired.Checked = true;
                }
                else if (rbwarr.Checked)
                {
                
                }
            }


        }
        // VERIFICA FAERIE FIRE (DRUID)
        bool isfire(Color cor, int slot)
        {
                if (cor.R == 11 && cor.G == 3 && cor.B == 11)
                return true;
                return false;
            

        }
        // VERIFICA DRAIN SOUL / DRAIN LIFE (Warlock) 
        bool isdrain(Color cor, int slot=1)
        {
            if (iscor(cor,45,111,256)) // B=136
                return true;
            return false;

        }

        // VERIFICA IMMOLATE (WARLOCK) no slot de debuff dado
        bool isimmo(Color cor, int slot)
        {
                if (cor.R == 0 && cor.G == 0 && cor.B == 0)
                return true;
                return false;
        
        }
   




        
        // VERIFICA CURSE OF WEAKNES (WARLOCK) no slot de debuff dado
        bool isweak(Color cor, int slot)
        {
              if ((cor.R == 21 && cor.G == 128 && cor.B == 43) ||
                        iscor(cor,233,98,256))

                    return true;
                return false;
            
           
        }
        // VERIFICA CORRUPTION (WARLOCK) no slot de debuff dado
        bool iscorr(Color cor, int slot)
        {
                if (cor.R == 109 && cor.G == 92 && cor.B == 93)
                    return true;
                return false;
            
        }
        // VERIFICA SE O PET ESTÁ SUMONADO / VIVO
        bool vepet()
        {
            if (rblock.Checked)
            {
                Color tempcor = vecor(110, 129); //(warlock)
                if (tempcor.R == 250 && tempcor.G == 200 && tempcor.B == 83) return true; // IMP
                //Não tem imp. Tem voidwalker?
                return (veiscor(114, 111, 78, 72, 256)); // tem voidwalker?
            }
            return false;
        }

        bool immo = false;
        bool rend = false;
        bool thun = false;
        bool demo = false;
        bool hams = false;
        bool weak = false;
        bool corr = false;
        bool drain = false;
        bool fire = false; // faerie fire debuff
        bool tempet = false;
        // WARLOCK VARIABLES
        int killed = 0;
        void sendstring(string phrase,int mod=0) // 0=raw 1=say 2=whisper myself
        {
            if (mod == 1) aperta(ENTER);
            phrase = "teste";
            for (int i=0;i<phrase.Length;i++)
            {
                aperta((byte)phrase[i]);
            }
            if (mod == 1) aperta(ENTER);
        }
        //----------------------------------
        // WAIT WHILE LOOTING
        //---------------------------------
        bool waitloot(int segundos) // WAIT N seconds or til life up
        {
            checkme();
            bool deucerto = true;
            for (int i = 0; i < segundos; i++)
            {
                wait(1000);
                checkme();
                if (me.combat) { deucerto = false; break; }
                if (me.tarhp==0) { return deucerto; }
                
            }
            return deucerto;

        }
        void loot()
        {
            killed++;
            checkme();
            if (!me.combat) aperta(N0, 200); // TARGET LAST TARGET
            checkme();
            if (!me.combat && !me.tarcombat && me.tarhp!=100 && me.tarhp==0) aperta(IKEY);
            waitloot(4);

            bool achou = true;
            focawow();
            checkme();
            if (rbdruid.Checked && me.hp < 85) cast(OITO); // cura 
            if (rbdruid.Checked)
            {
                mousemove(953, 752);
                DoMouseClick(); ;
                if (mudoucursor(797, 717)) goto fim;
                DoMouseClick();
                if (mudoucursor(770, 646)) goto fim;
                DoMouseClick();
                if (mudoucursor(882, 563)) goto fim;
                DoMouseClick();
                if (mudoucursor(958, 587)) goto fim;
                DoMouseClick();
                if (mudoucursor(1111, 635)) goto fim;
                DoMouseClick();
                if (mudoucursor(1229, 706)) goto fim;
            }
            else // warlock, hunter, warrior...
            {
                if (cbskin.Checked && !me.combat)
                {
                    loga("Searching body");
                    mousemove(958, 808); // clica no pé
                    DoMouseClick(); ;
                    if (mudoucursor(957, 506)) goto fim; // clica em cima
                    DoMouseClick();
                    if (mudoucursor(822, 580)) goto fim;
                    DoMouseClick();
                    if (mudoucursor(1108, 654)) goto fim;
                    DoMouseClick();
                    //if (mudoucursor(1214, 569)) goto fim;
                }
            }

            achou = false; ;
        fim:
            if (achou)
            {
                
                checkme();
                if (cbskin.Checked && !me.combat)
                {
                    wait(500);
                    loga("Skinning / Looting extra");
                    DoMouseClick(2);
                    waitloot(4); espera(2); }// click loot 

            }
            come();
            


        }
        void loga (string log)
        {
            lblog.Items.Insert(0, log);
            lblog.SelectedIndex = 0;
            if (lblog.Items.Count >= 9)
                 lblog.Items.RemoveAt(8);
                
           
        }
        void bebe(int sede=100)
        {
            checkme();
            if (me.mana < sede)
            {
                if (me.spd > 0) loga("Stop to drink");
                while (me.spd >0)
                {
                    aperta(WKEY, 2); checkme();
                    wait(100);
                }
                lblog.Items.Insert(0, "Drinking: " +sede.ToString());
                aperta(F3);
                espera(20, 101);
            }
        }
        bool come(bool agua=false)
        {
            
            if (me.hp == 100 || me.morreu) return false;
            if (!me.morreu)
            {
                bool comendo = false;
                //if (me.hp <60 && cbbandage.Checked) { aperta(F1); espera(6, 100); }
                checkme();
                if (me.morreu) return false;
                int foodhp = Convert.ToInt32(tbfoodhp.Text);
                

                if (me.hp < foodhp)
                { 
                  if (me.spd > 0) aperta(WKEY, 2); // para de andar
                    wait(500);
                    bebe(30);
                    if (rbpala.Checked && me.mana >= 30)
                    {
                        while (me.hp < foodhp && !me.combat && me.mana > 30) 
                        {
                            aperta(WKEY, 2);
                            wait(500);
                            aperta(OITO); 
                            espera(3,95);
                            checkme();
                            if (me.morreu) return false;
                        } 

                    }
                    else if (cbeat.Checked)
                    {
                        wait(500);
                        if (me.spd == 0) { aperta(F2); comendo = true; }
                        checkme();
                        if (comendo && me.spd == 0) espera(29, 95);
                    }
                    
                }// come
                if (cbdrink.Checked)
                    wait(500);
                bebe(Convert.ToInt32(tbmanahp.Text));


            }
            return true;
        }
        int maisperto(ref ListBox list,loc local)
        {
            int closest = -1;
            int myindex = 0;
            int nearestindex = 0;
            double toofar = 1000;
            loc oldloc;
            oldloc.x = 1000; oldloc.y = 1000; // longe demais 
            foreach (var listBoxItem in list.Items) // corre todos os waypoints até achar o mais perto
            {
                unpack(listBoxItem.ToString());
                if (distab(local,go) < distab(local,oldloc)) // nova distancia é menor
                {
                    oldloc.x = go.x; oldloc.y = go.y; // atualiza a nova distancia 
                    nearestindex = myindex;
                }

                myindex++;
                wait(20);
            }
            return nearestindex;
        }
        void puxa()
        {
            aperta(ZERO); // TAB ... /cleartarget /targetenemy 
            checkme();
            bool passa = false; // não ataca se for true 
            if (me.combat) passa = true;
            if (cbatacatrivial.Checked) passa = false; // ataca tudo.. não passa trivial 
            else if (me.level > me.tarlevel + 4) passa = true; // nao puxa cinza 
            if (!me.meleerange && rblock.Checked) passa = true;// ranged out of range 
            if (!me.rangedrange && rbhunta.Checked) passa = true;
            if (me.level + 1 < me.tarlevel && !cbhigh.Checked) passa = true; // só vai puxar um nivel acima do meu...
            // ------------------------
            // SELECT FAMILYS TO PULL
            //-----------------------
            if (!cbhumanoid.Checked && me.type == HUMANOID) passa = true;
            if (!cbmechanical.Checked && me.type == MECHANICAL) passa = true;
            if (!cbbeast.Checked && me.type == BEAST) passa = true;
            if (!cbelemental.Checked && me.type == ELEMENTAL) passa = true;
            if (!cbgiant.Checked && me.type == GIANT) passa = true;
            if (!cbelite.Checked && (me.type == ELITE || me.type ==251)) passa = true;
            if (!cbundead.Checked && me.type == UNDEAD) passa = true;
            //----------------------------------
            // PULL MOB
            //---------------------------------
            if (!passa && me.tarhp==100) loga("Mob found: Pulling");
            // WARRIOR PULL ROUTINE
            //---------------------------------
            if (rbwarr.Checked && !passa) // Warrior Pull
            {
                bool deucharge = false;
                checkme();

                // STUCK CHECK ROUTINE 
                int ticker = 0;
                loc oldpos;
                oldpos.x = 100; oldpos.y = 100; // initialize position 
                //-----------

               
                while (!me.combat && me.tarhp==100) 
                {
                    ticker++; // stuck checking ticker. 
                    checkme();
                    aperta(ZERO);
                    //aperta(SPACEBAR);
                    if (me.rangedrange && veiscor(592, 993, 222, 215, 158)) // range do judge , judge up
                    {
                        cast(N1);
                        aperta(WKEY, 2); // STOP MOVING 
                        deucharge = true;
                        aperta(UM);
                    } // CHARGE
                    if (!me.rangedrange) aperta(IKEY);
                    checkme();
                    if (deucharge) aperta(SKEY); 
                    if (deucharge && me.combat) { aperta(WKEY); } // para de andar 
                    else if (deucharge && !me.combat) { aperta(WKEY, 0); deucharge = false; }// bugou o charge

                    // STUCK CHECK ROUTINE ----------
                    if (ticker % 5 == 0 && !me.combat) // once every 4 ticks
                    {
                        if (me.pos.x == oldpos.x && me.pos.y == oldpos.y && !me.combat) unstuck(); // enroscou 
                        oldpos = me.pos;
                        aperta(SPACEBAR, 100); // dá pulinhos aleatorios 

                    }
                    
                    //----------------------
                }
                checkme();
                if (me.combat || me.meleerange)
                {
                    aperta(WKEY, 2); // STOP MOVING 
                    aperta(SPACEBAR); // JUMP
                    if (!deucharge) aperta(QKEY, 1000);
                    //aperta(IKEY); // FACE TARGET
                }
            }
            //---------------------------------
            // PALADIN PULL ROUTINE
            //---------------------------------
            else if (rbpala.Checked && !passa)
            {
                bool deucharge = false;
                checkme();

                // STUCK CHECK ROUTINE 
                int ticker = 0;
                loc oldpos;
                oldpos.x = 100; oldpos.y = 100; // initialize position 
                  
                while (!me.combat && me.tarhp == 100)
                {
                    ticker++; // stuck checking ticker. 
                    checkme();
                    // CHECK AND CAST SEAL OF RIGHTEOUS
                    if (!sor() && me.mana >= 14) aperta(DOIS); // SOR

                    aperta(ZERO); // tab target 
                    aperta(SPACEBAR); // jump 
                    if (me.rangedrange) // range do Judgement.... 
                    {
                        if (me.level >= 21 && me.type == UNDEAD && me.mana > 30 &&
                            veiscor(737, 988, 255, 254, 6)) aperta(N4); // exorcism
                        else if (sor()) aperta(N1); // judgement 
                        aperta(WKEY, 2); // STOP MOVING 
                        deucharge = true; // judged 
                        aperta(UM); // startattack 
                    } // CHARGE
                    aperta(IKEY); // interact with target (face and move) 
                    if (vecor(PLAYERSTATSX, PLAYERSTATSY).B == 255) me.combat = true; // check new position 
                    if (deucharge && !me.combat) { aperta(WKEY, 0); deucharge = false; }// bugou o judge
                    if (me.combat) { aperta(WKEY, 2); aperta(SKEY); }
                
                
                    // STUCK CHECK ROUTINE ----------
                    if (ticker % 5 == 0 && !me.combat) // once every 4 ticks
                    {
                        if (me.pos.x == oldpos.x && me.pos.y == oldpos.y && !me.combat) unstuck(); // enroscou 
                        oldpos = me.pos;
                        aperta(SPACEBAR, 100); // dá pulinhos aleatorios 
                    }

                    //----------------------
                }
                checkme();
                if (me.combat || me.meleerange) 
                {
                    aperta(WKEY, 2); // STOP MOVING 
                    //aperta(SPACEBAR); // JUMP
                    if (!deucharge) aperta(QKEY, 1000);
                    //aperta(IKEY); // FACE TARGET
                }
            }
            else if (!passa) // OTHER / RANGED CLASSES
            {
                {
                    if (cbstoptopull.Checked)
                    {
                        if (me.rangedrange && me.tarhp == 100
                            ) aperta(WKEY, 2); // PARA DE ANDAR
                        //wait(100);
                    }
                    if (me.tarhp == 100)
                    {
                        if (!rbwarr.Checked)
                        {
                            if (veiscor(334, 101, 0, 0, 97)) cast(N2); //Puxa caster 
                            else cast(N1); // Puxa Melee
                        }
                        else cast(N1);
                    }
                    checkme();
                    if (rbdruid.Checked) // melee class
                    {

                        if (me.combat)
                        {
                            cast(DOIS);

                        }
                    }
                }
            }
        }
        //----------------------------------
        // ANDA ATÉ A COORDENADA
        //---------------------------------
        bool enroscou = false;
        bool anda(bool vida=true)
        {
            
            checkme(); // lê minha posição atual 
            if (me.morreu || me.hp >= Convert.ToInt32(tbfoodhp.Text)|| (me.tarlevel >= me.level + 4)) // enough life to start
                aperta(WKEY, 0); // começa a andar 
            else aperta(WKEY, 2); // para de andar 

            int temp = 0;
            loc oldloc = me.pos;
            enroscou = false;
            int neardist = 0;
            if (me.morreu || (rbpala.Checked && me.level >= 22)) neardist = 6;
            else neardist = 5;
            while (dist(go) > neardist)
            {
                if (cbstop.Checked) return vida;
                if (me.morreu && vida) // click "release spirit" ou "Ressurect Now"
                {   aperta(F5);
                    return false;
                }
                // bebe depois do rez 
                come(); // se necessario. 
                checkme();
                if (!me.morreu && me.combat == true && !(me.tarlevel>=me.level+4)) // entrou em combate , não é elite
                {
                    aperta(WKEY, 2); // stop moving to fight 
                    combatloop(); // fight, loot, drink
                    aperta(WKEY, 0); // restart moving to look less botish
                }
                else if (!me.morreu) prepara(); // não... então se prepare 
                temp++;
                checkme();
                if (!me.combat) gira(go);
                //if (!me.combat)) wait(200);
                if (temp % 5 == 0 && !me.combat)
                {
                    aperta(SPACEBAR); // dá pulinhos aleatorios 
                }
                //---------------------------
                // CHECK IF PULL ROUTINE 
                //---------------------------
                if (!me.combat && !me.morreu) // vez sim vez não , ou seja a cada 2s
                {
                    checkme();
                    // CHECA SE PUXA
                    bool pull = false;
                    if (rbdruid.Checked)
                    {
                        if (me.mana > 40 || me.mana < 2) pull = true;
                    }
                    else if (rblock.Checked)
                    {
                        if (vepet() && me.mana > 20) pull = true; ; // só puxa se tiver pet 
                    }
                    else if (rbhunta.Checked)
                    {
                        pull = true;
                    }
                    else if (rbwarr.Checked)
                    {
                        pull = true;
                    }
                    else if (rbpala.Checked)
                    {
                        pull = true;
                    }
                    if (cbgrind.Checked && !me.combat && !me.morreu 
                        && pull) puxa();
                                                        
                                                                  //if (cbgrind.Checked) aperta(ZERO); // TENTA PUXAR ALGO
                    if (me.pos.x == oldloc.x && me.pos.y == oldloc.y && !me.combat) unstuck(); // enroscou 
                    oldloc = me.pos;
                }


            }
            //aperta(WKEY, 2);// SOLTA A tecla W

            return true;  
        }
        //----------------------------------
        // UNSTUCK SUBROUTINE
        //---------------------------------
        void unstuck()
        {

            if (!enroscou)
            {
                loga("Unstuck: Jump or Walk");
                aperta(WKEY, 0);
                aperta(SPACEBAR, 100);
            }
            if (enroscou) // (player.spd < 100)
            {
                loga("Unstuck: Turn around");
                aperta(WKEY);
                aperta(EKEY, 600);
                aperta(WKEY, 900);
            }
            aperta(WKEY, 0);
            enroscou = !enroscou;

        }
        //----------------------------------
        // VÊ A DISTÂNCIA ENTRE DOIS PONTOS
        //---------------------------------
        int distab(loc tar,loc orig)
        {
            double distance = Math.Round((Math.Sqrt(Math.Pow(Math.Abs(orig.x - tar.x), 2) +
            Math.Pow(Math.Abs(orig.y - tar.y), 2)))); // formula da distancia entre pontos
            return (int)distance; // converte double para int 

        }
        //----------------------------------
        // VÊ A DISTÂNCIA ATÉ UM PONTO
        //---------------------------------
        int dist(loc tar)
        {
            int distance = distab(tar, me.pos);
            tbdistance.Text = distance.ToString();
            return distance;
        }

        //----------------------------------
        // LÊ STATS DO PLAYER E TARGET 
        //---------------------------------
        void checkme()
        {
            if (!rbpala.Checked) vedebuffs(); // pala não tem põe debuff 
            if (!rbpala.Checked && !rbwarr.Checked) tempet = vepet(); // pala não tem pet 

            // -----------------------------------------------------------
            // LÊ DO ADDON - "STATUS", player level, player mana, is in combat 
            // ------------------------------------------------------------  
            Color cor = vecor(PLAYERSTATSX, PLAYERSTATSY); // level, mana, combat (status)
            me.level = Math.Round(cor.R / 4.25); // level 
            tblevel.Text = me.level.ToString();
            me.mana = (int)((double)cor.G / 2.55); // mana 
            if (cor.B == 255) me.combat = true; // em combat 
            else me.combat = false; // fora de combat 
            // -----------------------------------------------------------
            // LÊ pixels na tela....
            // ------------------------------------------------------------  
            if (veiscor(334, 101, 0, 0, 97)) me.tarcaster = true; else me.tarcaster = false;
            // -----------------------------------------------------------
            // LÊ CORPSE POSITION DO ADDON - CORPSE XY (x,y,--) batata
            // ------------------------------------------------------------   
            cor = vecor(1576, 288); // x trunc, x frac, speed
            me.corpse.x = cor.R * 10; // 0,0 == Not spirit
            me.corpse.y = cor.G * 10;
            tbcorpse.Text = pack(me.corpse);
            // -----------------------------------------------------------
            // LÊ DO ADDON - "COORDINATES" X SQUARE  (xint,xfloat, speed)
            // ------------------------------------------------------------   
            cor = vecor(PLAYERLOCXX, PLAYERLOCXY); // x trunc, x frac, speed
            double dcor = cor.G;
            dcor = dcor / 10;
            dcor = Math.Round(dcor);
            me.pos.x = cor.R * 10 + (int)dcor;
            me.spd = (int)((double)cor.B * 3.85); // speed 0-100
// -----------------------------------------------------------
// LÊ DO ADDON - "COORDINATES" Y SQUARE 
// ------------------------------------------------------------   

            cor = vecor(PLAYERLOCYX, PLAYERLOCYY);  // y trunc, y frac, facing 0-2000
            
            dcor = cor.G;
            dcor = dcor / 10;
            dcor = Math.Round(dcor);
            me.pos.y = cor.R * 10 +(int) dcor; // Converte para int 100x maior 

            
            me.face = (int)Math.Round(cor.B * 7.84);   // Lê facing (do pixel) 0-2000

            // -----------------------------------------------------------
            // LÊ DO ADDON - "TARGET HEALTH" SQUARE (melee range, target level, target hp)
            // ------------------------------------------------------------   
            cor = vecor(TARGETX, TARGETY); // lê cor coordenadas do pixel no addon range 
            me.tarhp = (int)((double)cor.B / 2.55); // range, level, health target = target health
            if (cor.R > 200) me.meleerange = true; else me.meleerange = false; // target stats, red 
            me.tarlevel = Math.Round(cor.G / 4.25); // level do mob
            tbtarlevel.Text = me.tarlevel.ToString();

            if (me.level >= me.tarlevel + 3) me.trivial = true; // 3 niveis abaixo é mob trivial 
            else me.trivial = false;

            // -----------------------------------------------------------
            // LÊ DO ADDON - "PLAYER HEALTH" SQUARE RED=(mechanical/elemental = 1; NOCOMBAT TARGET=, ranged range, player hp)
            // ------------------------------------------------------------
            cor = vecor(MEX, MEY); // lê cor coordenadas do pixel no addon range 
            me.hp = (int)((double)cor.B / 2.55); // my hp  
            if (me.hp == 0) { me.morreu = true; cbdead.Checked = true; } 
            else { cbdead.Checked = false; me.morreu = false; }
            me.humanoid = me.lifeless = false; me.tarcombat = true; me.rangedrange = false; // RESET FLAGS
            if (cor.G > 250) me.rangedrange = true; ; // ranged range
            if (me.combat && cor.R > 200) me.lifeless = true; // 1
            else if  (me.combat && (cor.R == 77 || cor.R==78)) me.humanoid = true;  // 0.3
            else if (cor.R == 128) me.tarcombat = false; // 0.5

            // -----------------------------------------------------------
            // LÊ DO ADDON - "MOB COUNT" SQUARE (visible health barscount) 1578x332, G=family
            // ------------------------------------------------------------ 
            // 1 mob = 26, 2 mob = 51, 3 mob = 77
            cor = vecor(1578, 332);
            if (cor.R == 26) me.mobcount = 1;
            else if (cor.R == 51) me.mobcount = 2;
            else if (cor.R == 77) me.mobcount = 3;
            else if (cor.R > 77) me.mobcount = 4;
            else if (cor.R == 0) me.mobcount = 0;

            // READ MOB FAMILY
            me.type = cor.G;
            tbfamily.Text = me.type.ToString();
            

            // VERIFICAÇÕES EXCLUSIVAS DE WARLOCK 
            if (rblock.Checked)
            {
                //

            }
            if (debug)
            {
                tbhp.Text = me.hp.ToString();
                tbmana.Text = me.mana.ToString();
                tbcombat.Text = me.combat.ToString();
                tbspd.Text = me.spd.ToString();
                tbx.Text = me.pos.x.ToString();
                tby.Text = me.pos.y.ToString();
                tbface.Text = me.face.ToString();
                tbtarhp.Text = me.tarhp.ToString();
                tblevel.Text = me.level.ToString();
                tbinrange.Text = "Melee: " + me.meleerange.ToString() + " Ranged: " + me.rangedrange.ToString();
            }
            
        }
        //----------------------------------
        // ROTINAS DE WARLOCK 
        //---------------------------------
        bool temhs() // tem healthstone? 
        {
            return veiscor(881, 995, 255, 254, 65);

        }

        //----------------------------------
        // WAIT WHILE OUT OF COMBAT ROUTINE
        //---------------------------------
        bool espera(int segundos,int hp=0) // WAIT N seconds or til life up
        {
            checkme();
            bool deucerto = true;
            bool tavandando = false;
            if (me.spd > 0) { tavandando = true; aperta(WKEY, 2); } // para de andar
            checkme();
                for (int i=0;i<segundos;i++)
                {
                    checkme();
                if (me.combat) { deucerto = false; break; }
                if (hp == 101 && me.mana > 90) break;
                if (hp!= 101 && hp>0 && me.hp >= hp) { break; }
                    wait(1000);
                }
            //if (tavandando) aperta(WKEY, 0);
            return deucerto;

        }
        //----------------------------------
        // PREPARA PARA COMBATE
        //---------------------------------
        void prepara()
        {

            checkme();
            if (!me.morreu) // nao vai bufar se estiver morto
            {
                //----------------------------------
                // WARLOCK PREPARATION
                //---------------------------------
                if (rblock.Checked)// PREPARAÇÃO DE WARLOCK
                {
                    if (false)// !veiscor(1113, 985, 35, 18, 12)) // soustone up
                    {
                        cast(WKEY, 2); wait(100);
                        clica(1112, 993); wait(4000);
                        clica(1118, 1053);
                        clica(1118, 1053); wait(4000);
                    }
                    if (!tempet)
                    {
                        cast(WKEY, 2); // para de andar 
                        aperta(NOVE, 11000);
                    }
                    //----------------------------------
                    // CHECK BUFFS AT HEALBOT BAR
                    //---------------------------------
                    if (vecor(HBX, HBY).G > 200 && me.mana > 50) // check missing buffs at healbot bar
                    {
                        clica(HBX, HBY); // click healbot bar 
                    }
                    //----------------------------------
                    // CHECK MANA AND HP (EAT/DRINK)
                    //---------------------------------
                    if (me.mana < me.hp && me.hp > 80) cast(CINCO); // life tap
                    checkme();
                    if (me.mana < 40) // low mana 
                    {
                        cast(WKEY, 2); // para de andar 
                        if (!me.combat) cast(F1, 11000); // DRINK

                    }
                    checkme();
                    come();
                    if (rblock.Checked && !veiscor(179, 135, 256, 134, 256)) // pet morrendo 
                    {
                        if (!me.combat && me.level >= 18) { clica(994, 911); wait(2000); }// consume shadows
                        //if (me.hp > 60 && me.combat) cast(N3, 4000); // health funnel em combate
                    }
                    
                    //----------------------------------
                    // CHECK CONJURED ITENS
                    //---------------------------------
                    checkme();
                    if (!temhs()) // sem healthstone 
                    {
                        cast(WKEY, 2); // para de andar 
                        if (!me.combat) cast(N7, 3100); // casta healthstone
                    }
                }
                //----------------------------------
                // DRUID PREPARATION
                //---------------------------------
                else if (rbdruid.Checked)
                {
                    if (me.hp < 70) cast(TRES, 1500); // cura se vida baixa 
                    //----------------------------------
                    // CHECK BUFFS AT HEALBOT BAR
                    //---------------------------------
                    if (vecor(HBX, HBY).G > 200 && me.mana > 50) // check missing buffs at healbot bar
                    {
                        clica(HBX, HBY); // click healbot bar 
                    }// se hb bar verde, bufa 
                }
                //----------------------------------
                // HUNTER COMBAT PREPARATION  - HCP
                //---------------------------------
                else if (rbhunta.Checked)
                {

                }
                //----------------------------------
                // WARRIOR COMBAT PREPARATION - WCP
                //---------------------------------
                else if (rbwarr.Checked)
                {
                  
                    if (vecor(1799, 303).G > 200 && me.mana >= 10) cast(NOVE); // BATTLE SHOUT 
                    come();
                }
                //----------------------------------
                // PALADIN COMBAT PREPARATION - WCP
                //---------------------------------
                else if (rbpala.Checked)
                {
                    
                    if (!sor() && me.mana >= 25) cast(DOIS); // SOR
                    if (!might() && me.mana >= 25) cast(QUATRO); // SOR
                    come();
                }

                //checkme(); // get player stats 
                
            }
        }



	
	//----------------------------------
	// VE SE MUDOU CURSOR NAS NOVAS COORDENADAS
	//---------------------------------
	bool mudoucursor(int x, int y)
        {

	          public static CURSORINFO oldmouse; // Variavel global para o metodo abaixo   
	          DoMouseClick();
            wait(200);
            oldmouse.hCursor = getcursor();
            mousemove(x, y);
            DoMouseClick();
            wait(200);
            if (oldmouse.hCursor == getcursor()) return false;
            return true;
        }

        //----------------------------------
        // ESTRUTURAS / STRUCT
        //---------------------------------
 
        // estrutura do status dos bonecos no jogo

        public static loc go;
        public struct mob
        {
            public loc pos;
            public int hp;
            public int spd;
            public int face;
            public bool combat;
            public int mana;
            public double level;
            public int tarhp;
            public bool meleerange;
            public bool rangedrange;
            public bool trivial;
            public double tarlevel;
            public bool lifeless;
            public bool humanoid;
            public bool tarcaster;
            public bool tarcombat;
            public int mobcount;
            public int type;
            public bool morreu;
            public loc corpse;
            
            

        }
        //-------------------------
        // Estrutura de coordenada 
        //-------------------------
        public struct loc
        {
            public int x;
            public int y;
        }
        //----------------------------------
        // VARIAVEIS DINAMICAS
        //---------------------------------
        public static mob me;
        //Create a 3 channel image of 400x200
        public static CURSORINFO pci;

        public static bool debug = true;
        bool[,,,] tedr = new bool[999, 999, 10,2];
        //0=Is acessible
        //1=N
        //2=S
        //3=E
        //4=W
        //5=NE
        //6=NW
        //7=SE
        //8=SW
        //9=Unvisited
        static Bitmap bmtedr = new Bitmap(500, 500);

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        // botao check me
        private void button4_Click(object sender, EventArgs e)
        {
            checkme();

        }

        // PREPARA
        private void button5_Click(object sender, EventArgs e)
        {
            prepara();
            
        }
        static bool  rot = false;
        static bool  drot = false;
        //-------------------------
        // COMBAT LOOP 
        //-------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            cbskin.Checked = false; // desliga skinning 
            if (cbdrot.Checked) drot = true;
            rot = true;
            while (!cbstop.Checked) // loop infinito 
            {
                checkme();
                bool needloot = false;
                if (!me.combat) prepara();
                while (me.combat && !cbstop.Checked)
                {
                    
                    combatloop();
                    if (!drot) needloot = true;
                }
                if (needloot && !drot) loot();
                wait(500);


            }
            rot = false;
        }

        string pack(loc xy)
        {
            return (xy.x * 1000 + xy.y).ToString();
        }
        public void unpack(string pack)
        {
            try
            {
                int target = Convert.ToInt32(pack); // desempacota string de coordenada 
                loc tar;
                tar.x = (int)(double)(target / 1000);
                tar.y = target - tar.x * 1000;
                go.x = tar.x; go.y = tar.y;
                tbtx.Text = tar.x.ToString();
                tbty.Text = tar.y.ToString();
            }
            catch
            {
                MessageBox.Show("Erro ao carregar waypoints.");
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {

        }
        bool terminou;
        //-------------------------
        // START MOVING TO WAYPOITS
        //-------------------------
        private void button7_Click(object sender, EventArgs e)
        {

        }
        public void mainBotMethod()
        {



        }
        // ADD WAYPOINT button. Checks current position and adds to waypoint list. 
        private void button1_Click_1(object sender, EventArgs e)
        {

            checkme();
            lbwp.Items.Add(pack(me.pos)); // empacota coordenada e adiciona a lista
            lbwp.SelectedIndex = 0;

        }

        private void lbwp_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        // Clear waypoints button. 
        private void button2_Click_1(object sender, EventArgs e)
        {
            lbwp.Items.Clear();

        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            // SAVE LIVE WAYPOINTS
            string sPath = tbfilename.Text + ".txt";
            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(sPath);
            foreach (var item in lbwp.Items)
            {
                SaveFile.WriteLine(item);
            }
            SaveFile.Close();

            // SAVE GHOST WAYPOINTS
            sPath = tbfilename.Text + "-ghost.txt";
            System.IO.StreamWriter SaveFileG = new System.IO.StreamWriter(sPath);
            foreach (var item in lbgwp.Items)
            {
                SaveFileG.WriteLine(item);
            }
            SaveFileG.Close();
            MessageBox.Show("Waypoints saved!");
        }

        // LOAD button
        void carrega()
        {
            // LOAD LIVE WAYPOINTS
            lbwp.Items.Clear(); // limpa x 
            var lines = File.ReadAllLines(tbfilename.Text + ".txt"); // le X do arquivo 
            lbwp.Items.AddRange(lines);
            try { lbwp.SelectedIndex = 0; } catch { } // seleciona ultimo item

            try
            {
                // LOAD GHOST WAYPOINTS
                lbgwp.Items.Clear(); // limpa x 
                lines = File.ReadAllLines(tbfilename.Text + "-ghost.txt"); // le X do arquivo 
                lbgwp.Items.AddRange(lines);
                try { lbgwp.SelectedIndex = 0; } catch { } // seleciona ultimo item
            }
            catch { }
        }
        private void button6_Click_2(object sender, EventArgs e)
        {
            carrega(); 
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            lbwp.Items.Add(tbwaypoint.Text);
            lbwp.SelectedIndex = 0;
            tbwaypoint.Text = "";
        }
        // -------------------------------------
        // SALVA MATRIZ PARA ARQUIVO .DAT 
        // -------------------------------------
        void saveit(bool[,,,] matriz, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            // writing
            using (FileStream fs = new FileStream(filename, FileMode.Create))
                bf.Serialize(fs, matriz);
        }
        // -------------------------------------
        // SALVA SETUP PARA ARQUIVO  cfg
        // -------------------------------------
        void savesetup(setup matriz, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            // writing
            using (FileStream fs = new FileStream(filename, FileMode.Create))
                bf.Serialize(fs, matriz);
        }
        // -------------------------------------
        // CARREGA SETUP DE ARQUIVO  CFG
        // -------------------------------------
        void loadsetup(ref setup matriz, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            // reading
            using (FileStream fs = new FileStream(filename, FileMode.Open))
                matriz = (setup)bf.Deserialize(fs);
        }
        // -------------------------------------
        // CARREGA MATRIZ PARA ARQUIVO .DAT 
        // -------------------------------------
        void loadit(ref bool[,,,] matriz, string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            // reading
            using (FileStream fs = new FileStream(filename, FileMode.Open))
                matriz = (bool[,,,])bf.Deserialize(fs);
        }

        // SAVE TETRASSIL VARIABLE TO BIN FILE 
        private void button9_Click(object sender, EventArgs e)
        {
            saveit(tedr, "tedr.bin");
        }

        // LOAD TEDRASSIL BIN FILE TO VARIABLE 
        private void button10_Click(object sender, EventArgs e)
        {
            loadit(ref tedr, "tedr.bin");
        }

        private void button11_Click(object sender, EventArgs e)
        {


        }

        private void button12_Click(object sender, EventArgs e)
        {
            pbtedr.Image = bmtedr;
        }

        void lemap()
            {
            //checkme();
            /*
            lb1.Text = tedr[me.pos.x, me.pos.y, 1,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 1, 1]) lb1.Text = "-"; // posição não testada 
            lb2.Text = tedr[me.pos.x, me.pos.y, 2,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 2, 1]) lb2.Text = "-"; // posição não testada 
            lb3.Text = tedr[me.pos.x, me.pos.y, 3,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 3, 1]) lb3.Text = "-"; // posição não testada 
            lb4.Text = tedr[me.pos.x, me.pos.y, 4,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 4, 1]) lb4.Text = "-"; // posição não testada 
            lb5.Text = tedr[me.pos.x, me.pos.y, 5,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 5, 1]) lb5.Text = "-"; // posição não testada 
            lb6.Text = tedr[me.pos.x, me.pos.y, 6,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 6, 1]) lb6.Text = "-"; // posição não testada 
            lb7.Text = tedr[me.pos.x, me.pos.y, 7,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 7, 1]) lb7.Text = "-"; // posição não testada 
            lb8.Text = tedr[me.pos.x, me.pos.y, 8,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 8, 1]) lb8.Text = "-"; // posição não testada 
            lb9.Text = tedr[me.pos.x, me.pos.y, 9,0].ToString();
            if (!tedr[me.pos.x, me.pos.y, 9, 1]) lb9.Text = "-"; // posição não testada 
    */
            tbdx.Text = me.pos.x.ToString();
            tbdy.Text = me.pos.y.ToString();
            
            g.Clear(Color.White);
            for (int i=0;i<50;i++)
            {
                for (int j=0;j<50;j++)
                {
                    
                    if (tedr[me.pos.x-25+i,me.pos.y-25+j,5,0] 
                        && tedr[me.pos.x - 25 + i, me.pos.y - 25 + j, 5, 1]  )
                    pinta(Color.Blue, i*10, j*10);
                }
            }
            pinta(Color.Red, 250, 250);

        }
        private void button13_Click(object sender, EventArgs e)
        {
            //loadit(ref tedr, "tedr.bin"); // carrega arquivo 
            //pinta(Color.Red, 200, 200);

            btrecord.ForeColor = Color.Red; // seta cor de eque está gravando 
            loc oldpos;
            checkme();
            oldpos.x = me.pos.x;
            oldpos.y = me.pos.y;
            lemap();
           
            while (!cbstop.Checked)
            {
                checkme();
                tedr[me.pos.x, me.pos.y, 5, 0] = true; // current pos = acessible
                tedr[me.pos.x, me.pos.y, 5, 1] = true; // acessible

                if ((Math.Abs(oldpos.x - me.pos.x)<5 && Math.Abs(oldpos.y - me.pos.y) < 5)
                && (oldpos.x != me.pos.x || oldpos.y != me.pos.y)) // andou 1 ponto 
                {
                    bool subiux = false; bool subiuy = false;
                    bool desceux = false; bool desceuy = false;
                    if (me.pos.x > oldpos.x) subiux = true;
                    if (me.pos.y > oldpos.y) subiuy = true;
                    if (me.pos.x < oldpos.x) desceux = true;
                    if (me.pos.y < oldpos.y) desceuy = true;
                    
                    
                    if (subiuy && desceux)
                    {
                        tedr[oldpos.x, oldpos.y, 1, 0] = true; // NW
                        tedr[oldpos.x, oldpos.y, 1, 1] = true; // Checked
                    }
                    if (subiuy && !subiux)
                    {
                        tedr[oldpos.x, oldpos.y, 2, 0] = true; // N
                        tedr[oldpos.x, oldpos.y, 2, 1] = true; // Checked
                    }
                    if (subiuy && subiux)
                    {
                        tedr[oldpos.x, oldpos.y, 3, 0] = true; // NE
                        tedr[oldpos.x, oldpos.y, 3, 1] = true; // checked
                    }
                    if (desceux && !subiuy)
                    {
                        tedr[oldpos.x, oldpos.y, 4, 0] = true; // W
                        tedr[oldpos.x, oldpos.y, 4, 1] = true; // Checked
                    }
                    if (!subiuy && !subiux)
                    {
                        tedr[oldpos.x, oldpos.y, 6, 0] = true; // E
                        tedr[oldpos.x, oldpos.y, 6, 1] = true; // Checked
                    }
                    if (desceuy && desceux)
                    {
                        tedr[oldpos.x, oldpos.y, 7, 0] = true; // SW
                        tedr[oldpos.x, oldpos.y, 7, 1] = true; // checked
                    }
                    if (desceuy && !desceux)
                    {
                        tedr[oldpos.x, oldpos.y, 8, 0] = true; // W
                        tedr[oldpos.x, oldpos.y, 8, 1] = true; // Checked
                    }
                    if (!desceuy && subiux)
                    {
                        tedr[oldpos.x, oldpos.y, 9, 0] = true; // SE
                        tedr[oldpos.x, oldpos.y, 9, 1] = true; // Checked
                    }
                    
                    //bmtedr.SetPixel(me.pos.x, me.pos.y, Color.White);
                    //pbtedr.Image = bmtedr;
                    lemap();
                }
                oldpos = me.pos;
                lemap();
                wait(1);

            }
            btrecord.ForeColor = Color.Black;
            saveit(tedr, "tedr.bin"); // salva arquivo de mapa
            //bmtedr.Save("tedrassil.bmp");
        }
        // BOTÃO FIND COLOR
        private void button14_Click(object sender, EventArgs e)
        {
            
           
        }

        // botão FIND NEXT
        private void button15_Click(object sender, EventArgs e)
        {
            
                          
          
        }
        // VECOR AT XY BUTTON
        private void button16_Click(object sender, EventArgs e)
        {
            int mx = Convert.ToInt32(tbgxmx.Text);
            int my = Convert.ToInt32(tbgxmy.Text);
            Color tempcolor = vecor(mx, my);
            tbc.Text = tempcolor.ToString();
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            Cursor = new Cursor(Cursor.Current.Handle);
            //Cursor.Position = new Point(Cursor.Position.X - 50, Cursor.Position.Y - 50);
            int mx = Cursor.Position.X;
            int my = Cursor.Position.Y;
            tbgxmx.Text = Cursor.Position.X.ToString();
            tbgxmy.Text = Cursor.Position.Y.ToString();
            Color tempcolor = vecor(mx, my);
            tbc.Text = tempcolor.ToString();
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            int mx = Convert.ToInt32(tbgxmx.Text);
            int my = Convert.ToInt32(tbgxmy.Text);
            int iy = my; int ix = mx;
            for (; mx <400;mx++)
            {
                Color tcor = vecor(mx, my);
                tbgxmx.Text = mx.ToString();
                tbgxmy.Text = my.ToString();
                tbc.Text = tcor.ToString();
                if (iscorr(tcor,1)) break;
                if (mx > 400) mx = ix;
                wait(100);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            vedebuffs();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            for (int i=0;i<9;i++)
            {
                for(int j = 0; j < 9;j++)
                {
                    tedr[i,i,j,0] = false;
                    tedr[i, i, j, 1] = false;
                }
            }
            MessageBox.Show("Variable Cleared.");
        }
        // Marca ponto no bitmap
        Graphics g = Graphics.FromImage(bmtedr);
        void pinta(Color cor, int x, int y)
        {

           
            if (cor==Color.Blue) g.FillRectangle(Brushes.Blue, x, y, 10, 10);
            if (cor == Color.Red) g.FillRectangle(Brushes.Red, x, y, 10, 10);
            if (cor == Color.Black) g.FillRectangle(Brushes.Red, x, y, 10, 10);
            if (cor == Color.White) g.FillRectangle(Brushes.Red, x, y, 10, 10);
            pbtedr.Image = bmtedr;
        }
        // DRAW FLAG BUTTON
        private void button18_Click(object sender, EventArgs e)
        {
            pinta(Color.Red, 250, 250);
        }

        // GIRA - BOTAO DEBUG
        private void button19_Click(object sender, EventArgs e)
        {
            checkme();
            string temp = lbwp.SelectedItem.ToString();
            unpack(temp); // unpack waypoint from listbox 
            gira(go);
        }

        // botao FALA
        private void button20_Click(object sender, EventArgs e)
        {

            sendstring(tbc.Text,1);
            
            
        }
        bool testcor(Color cor,int r,int g,int b,int tol=10)
        {
            bool result = true;
            if (cor.R > r + tol || cor.R < r - tol) return false;
            if (cor.G > g + tol || cor.G < g - tol) return false;
            if (cor.B > b + tol || cor.G < b - tol) return false;
            return true;
        }
        // Botão SCAN MINIMAP
        private void button21_Click(object sender, EventArgs e)
        {
           
        }
        // ADD GHOST WAYPOINT BUTTON
        private void button15_Click_2(object sender, EventArgs e)
        {
            checkme();
            lbgwp.Items.Add(pack(me.pos)); // empacota coordenada e adiciona a lista
            lbgwp.SelectedIndex = 0;
        }
        // BOTÃO CLEAR GHOST WAYPOINTS
        private void button22_Click(object sender, EventArgs e)
        {
            lbgwp.Items.Clear();
        }

        // botão start threaded
        bool ghostwpdone = false;
        bool paratudo = false;
        int itens;
        int atual;
        
        private void button23_Click(object sender, EventArgs e)
        {
            tbfilename.Text = "default";
        }
        int andalista(ref ListBox lista,bool vida, int lastindex=999)
        {
            checkme();
            lista.SelectedIndex = maisperto(ref lista, me.pos);
            aperta(WKEY, 0);
            while (!cbstop.Checked)
            {
                unpack(lista.SelectedItem.ToString());
                if (!anda(vida)) return 2; // exist list if died/ressed
                if (lista.SelectedIndex == lastindex) return 1;
                try { lista.SelectedIndex += 1; }
                catch { lista.SelectedIndex = 0; }
            }
        return 1;
        }
        void startbot()
        {
        

        }
        // START BOT BUTTON
        private void button5_Click_1(object sender, EventArgs e)
        {
            if (lbwp.SelectedIndex == -1) carrega(); // load default 
            
            while (!cbstop.Checked)
            {
                checkme();
                if (!me.morreu)
                {
                    loga("Starting alive waypoints");
                    andalista(ref lbwp, true); // walk living list, alive, loop
                }
                else if (me.morreu)
                {
                    loga("Starting corpse run");
                    int corpseidx = maisperto(ref lbgwp, me.corpse); // spot near corpse
                    tbdebug1.Text = corpseidx.ToString();
                    tbdebug2.Text = me.corpse.x.ToString() + " " + me.corpse.y.ToString();
                    andalista(ref lbgwp, false, corpseidx); // walke dead list, dead, until corpseidx
                    loga("Walking to corpse.");
                    unpack(tbcorpse.Text); anda(false);
                    loga("Trying to accept rez.");
                    cast(F5); aperta(WKEY); prepara();
                }
                aperta(WKEY);
            }
        }
        //BUNNY JUMP METHOD
        void bunny(int order)
        {

            
            aperta(WKEY, order);
            aperta(EKEY, order);
            aperta(AKEY, order);

        }
        //BUNNY JUMP BUTTON
        bool topulando = false;
        private void button24_Click(object sender, EventArgs e)
        {
            int order; 
            topulando = !topulando;
            if (topulando) order = 2; else order = 0;
            bunny(order);
        }

        private void button25_Click(object sender, EventArgs e)
        {

            lbwp.Items.RemoveAt(lbwp.SelectedIndex);
            
        }

        private void button26_Click(object sender, EventArgs e)
        {
            lbgwp.Items.RemoveAt(lbgwp.SelectedIndex);
        }
        void vecomidas()
        {
            if (rbcautious.Checked) {  tbfoodhp.Text = "45"; }
            if (rbnormal.Checked) {  tbfoodhp.Text = "35"; }
            if (rbbersek.Checked) { tbfoodhp.Text = "25"; }
            if (cbauto.Checked) {  tbfoodhp.Text = "20"; }

        }
               
        private void rbnormal_CheckedChanged(object sender, EventArgs e)
        {
            vecomidas();
        }

        private void rbcautious_CheckedChanged(object sender, EventArgs e)
        {
            vecomidas();
        }

        private void rbbersek_CheckedChanged(object sender, EventArgs e)
        {
            vecomidas();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @".",
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = false,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string rawstring = openFileDialog1.SafeFileName;
                for (int i=0;i<4;i++)
                {
                    rawstring = rawstring.Remove(rawstring.Length - 1);
                }
                tbfilename.Text = rawstring;
            }
            carrega();
        }
        // --------------------
        // LOAD SETUP BUTTON
        // ---------------------
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        [Serializable]
        public struct setup
        {
            public bool[] cb;
            public string[] cbname;
            public bool[] rb;
            public string[] rbname;
            public string[] tb;
            public string[] tbname;
        }
        private void button28_Click(object sender, EventArgs e)
        {
            setup su = new setup(); // create "ram image" from to setup

            int i = -1; // will start from [0]
            foreach (Control c in Controls) // run all controls 
            {
                if (c is CheckBox) // start with checkbox 
                {
                    i++; // incrementa numero de checkbox 
                    if (((CheckBox)c).Checked) su.cb[i] = true; // checked 
                    else su.cb[i] = false; // add CB status to ram vector
                    su.cbname[i] = ((CheckBox)c).Name; // save name of CB
                }
                    
            }
            savesetup(su, "setup.cfg");

        }

        private void button29_Click(object sender, EventArgs e)
        {
            setup su = new setup(); // create "ram image" from to setup
            loadsetup(ref su, "setup.cfg");
            int i = -1;
            bool broken = false;
            foreach (Control c in Controls) // run all controls 
            {
                if (c is CheckBox && !broken) // start with checkbox 
                {
                    i++; // incrementa numero de checkbox 
                    //if (su.cbname[i] != (((CheckBox)c).Name))
                    //{ broken = true; break; } // file broken 
                    if (su.cb[i]) // checked 
                        ((CheckBox)c).Checked = true;  // check checkbox
                    else ((CheckBox)c).Checked = false;
                    ((CheckBox)c).Checked = false;

                }

            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    } 
    
}
