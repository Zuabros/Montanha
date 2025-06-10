using System.Windows.Forms;

namespace Discord
{
 partial class Form1
 {
	/// <summary>
	/// Variável de designer necessária.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Limpar os recursos que estão sendo usados.
	/// </summary>
	/// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
	protected override void Dispose(bool disposing)
	{
	 if (disposing && (components != null))
	 {
		components.Dispose();
	 }
	 base.Dispose(disposing);
	}

	#region Código gerado pelo Windows Form Designer

	/// <summary>
	/// Método necessário para suporte ao Designer - não modifique 
	/// o conteúdo deste método com o editor de código.
	/// </summary>
	private void InitializeComponent()
	{
	 this.tab_nav = new System.Windows.Forms.TabControl();
	 this.tabPage1 = new System.Windows.Forms.TabPage();
	 this.tb_mobs = new System.Windows.Forms.TextBox();
	 this.tb_timer_hours = new System.Windows.Forms.TextBox();
	 this.button18 = new System.Windows.Forms.Button();
	 this.button17 = new System.Windows.Forms.Button();
	 this.cb_assist_tank = new System.Windows.Forms.CheckBox();
	 this.cb_HS_timer = new System.Windows.Forms.CheckBox();
	 this.hs_min_left = new System.Windows.Forms.TextBox();
	 this.button15 = new System.Windows.Forms.Button();
	 this.find_dots = new System.Windows.Forms.Button();
	 this.button14 = new System.Windows.Forms.Button();
	 this.button10 = new System.Windows.Forms.Button();
	 this.tbraio = new System.Windows.Forms.TextBox();
	 this.tby = new System.Windows.Forms.TextBox();
	 this.tbx = new System.Windows.Forms.TextBox();
	 this.pb_minimap = new System.Windows.Forms.PictureBox();
	 this.label18 = new System.Windows.Forms.Label();
	 this.tb_back_limiar = new System.Windows.Forms.TextBox();
	 this.label9 = new System.Windows.Forms.Label();
	 this.label8 = new System.Windows.Forms.Label();
	 this.tbavdecay = new System.Windows.Forms.TextBox();
	 this.tbdecay = new System.Windows.Forms.TextBox();
	 this.rtbcursor = new System.Windows.Forms.TextBox();
	 this.button11 = new System.Windows.Forms.Button();
	 this.button7 = new System.Windows.Forms.Button();
	 this.bt_saveWPas = new System.Windows.Forms.Button();
	 this.tb_filename = new System.Windows.Forms.TextBox();
	 this.bt_debug3 = new System.Windows.Forms.Button();
	 this.button8 = new System.Windows.Forms.Button();
	 this.cb_anda = new System.Windows.Forms.CheckBox();
	 this.bt_saveWP = new System.Windows.Forms.Button();
	 this.cb_nostop = new System.Windows.Forms.CheckBox();
	 this.button6 = new System.Windows.Forms.Button();
	 this.tb_WP_distance = new System.Windows.Forms.TextBox();
	 this.button5 = new System.Windows.Forms.Button();
	 this.cb_round = new System.Windows.Forms.CheckBox();
	 this.button4 = new System.Windows.Forms.Button();
	 this.lb_log = new System.Windows.Forms.TextBox();
	 this.button3 = new System.Windows.Forms.Button();
	 this.button2 = new System.Windows.Forms.Button();
	 this.lbwp = new System.Windows.Forms.ListBox();
	 this.bt_onoff = new System.Windows.Forms.Button();
	 this.tabPage2 = new System.Windows.Forms.TabPage();
	 this.cb_scan_elite = new System.Windows.Forms.CheckBox();
	 this.cb_elite_patrol = new System.Windows.Forms.CheckBox();
	 this.cb_rare_patrol = new System.Windows.Forms.CheckBox();
	 this.tb_wait_patrol = new System.Windows.Forms.TextBox();
	 this.cb_humanoid_patrol = new System.Windows.Forms.CheckBox();
	 this.cb_giant_patrol = new System.Windows.Forms.CheckBox();
	 this.cb_wrong_gira = new System.Windows.Forms.CheckBox();
	 this.cb_loot_cloth = new System.Windows.Forms.CheckBox();
	 this.cb_hearth_bagfull = new System.Windows.Forms.CheckBox();
	 this.cb_nohumanoid = new System.Windows.Forms.CheckBox();
	 this.pb_map = new System.Windows.Forms.PictureBox();
	 this.cb_nodragonkin = new System.Windows.Forms.CheckBox();
	 this.label21 = new System.Windows.Forms.Label();
	 this.tb_pullcap = new System.Windows.Forms.TextBox();
	 this.lb_map_zoom = new System.Windows.Forms.Label();
	 this.tb_map_zoom = new System.Windows.Forms.TextBox();
	 this.tab_buffs = new System.Windows.Forms.TabControl();
	 this.Paladin = new System.Windows.Forms.TabPage();
	 this.cb_flashheal = new System.Windows.Forms.CheckBox();
	 this.tb_purify_delay = new System.Windows.Forms.TextBox();
	 this.cb_BOP = new System.Windows.Forms.CheckBox();
	 this.tb_JOTC_lvl = new System.Windows.Forms.TextBox();
	 this.cb_keep_JOTC = new System.Windows.Forms.CheckBox();
	 this.label13 = new System.Windows.Forms.Label();
	 this.tb_disable_BOW = new System.Windows.Forms.TextBox();
	 this.cb_keep_JOL = new System.Windows.Forms.CheckBox();
	 this.label12 = new System.Windows.Forms.Label();
	 this.tb_combatheal = new System.Windows.Forms.TextBox();
	 this.label10 = new System.Windows.Forms.Label();
	 this.tb_pull_mana = new System.Windows.Forms.TextBox();
	 this.cb_retaura = new System.Windows.Forms.CheckBox();
	 this.cb_devaura = new System.Windows.Forms.CheckBox();
	 this.tb_BOSA_limiar = new System.Windows.Forms.TextBox();
	 this.tb_SOL_limiar = new System.Windows.Forms.TextBox();
	 this.tb_mana_min = new System.Windows.Forms.TextBox();
	 this.cb_savemana = new System.Windows.Forms.CheckBox();
	 this.cb_SOL = new System.Windows.Forms.CheckBox();
	 this.cb_BOSA = new System.Windows.Forms.CheckBox();
	 this.cb_concentration_aura = new System.Windows.Forms.CheckBox();
	 this.cb_use_exorcism = new System.Windows.Forms.CheckBox();
	 this.cb_BOK = new System.Windows.Forms.CheckBox();
	 this.tb_bow_trig = new System.Windows.Forms.TextBox();
	 this.cb_BOW = new System.Windows.Forms.CheckBox();
	 this.tb_stoneform_at = new System.Windows.Forms.TextBox();
	 this.cb_loh = new System.Windows.Forms.CheckBox();
	 this.cb_dwarf = new System.Windows.Forms.CheckBox();
	 this.cb_bubble = new System.Windows.Forms.CheckBox();
	 this.cb_purify = new System.Windows.Forms.CheckBox();
	 this.label19 = new System.Windows.Forms.Label();
	 this.tb_interrupt_at = new System.Windows.Forms.TextBox();
	 this.cb_hammer_range = new System.Windows.Forms.CheckBox();
	 this.cb_use_hammer = new System.Windows.Forms.CheckBox();
	 this.label16 = new System.Windows.Forms.Label();
	 this.tb_preheal = new System.Windows.Forms.TextBox();
	 this.cb_judge_range = new System.Windows.Forms.CheckBox();
	 this.cb_judge = new System.Windows.Forms.CheckBox();
	 this.cb_BOM = new System.Windows.Forms.CheckBox();
	 this.cb_SOR = new System.Windows.Forms.CheckBox();
	 this.tabPage4 = new System.Windows.Forms.TabPage();
	 this.tb_evis_cp = new System.Windows.Forms.TextBox();
	 this.label24 = new System.Windows.Forms.Label();
	 this.label23 = new System.Windows.Forms.Label();
	 this.tb_combos = new System.Windows.Forms.TextBox();
	 this.pan_tar = new System.Windows.Forms.Panel();
	 this.cb_killgray = new System.Windows.Forms.CheckBox();
	 this.textBox1 = new System.Windows.Forms.TextBox();
	 this.cb_melee = new System.Windows.Forms.CheckBox();
	 this.label2 = new System.Windows.Forms.Label();
	 this.tb_tarcast = new System.Windows.Forms.TextBox();
	 this.pb_tarcast = new System.Windows.Forms.ProgressBar();
	 this.tb_mood = new System.Windows.Forms.TextBox();
	 this.label7 = new System.Windows.Forms.Label();
	 this.tb_tarlevel = new System.Windows.Forms.TextBox();
	 this.label6 = new System.Windows.Forms.Label();
	 this.tb_tartype = new System.Windows.Forms.TextBox();
	 this.tb_tarhp = new System.Windows.Forms.TextBox();
	 this.label1 = new System.Windows.Forms.Label();
	 this.pan_me = new System.Windows.Forms.Panel();
	 this.cb_apagacinza = new System.Windows.Forms.CheckBox();
	 this.cb_pacifist = new System.Windows.Forms.CheckBox();
	 this.cb_skinning = new System.Windows.Forms.CheckBox();
	 this.cb_loot = new System.Windows.Forms.CheckBox();
	 this.cb_herbalism = new System.Windows.Forms.CheckBox();
	 this.cb_pull = new System.Windows.Forms.CheckBox();
	 this.cb_autoattack = new System.Windows.Forms.CheckBox();
	 this.tb_playercast = new System.Windows.Forms.TextBox();
	 this.pb_playercast = new System.Windows.Forms.ProgressBar();
	 this.tb_level = new System.Windows.Forms.TextBox();
	 this.label5 = new System.Windows.Forms.Label();
	 this.cb_combat = new System.Windows.Forms.CheckBox();
	 this.tb_class = new System.Windows.Forms.TextBox();
	 this.label3 = new System.Windows.Forms.Label();
	 this.tb_hp = new System.Windows.Forms.TextBox();
	 this.label4 = new System.Windows.Forms.Label();
	 this.tb_mana = new System.Windows.Forms.TextBox();
	 this.tabPage3 = new System.Windows.Forms.TabPage();
	 this.button20 = new System.Windows.Forms.Button();
	 this.cb_logar_cura = new System.Windows.Forms.CheckBox();
	 this.label20 = new System.Windows.Forms.Label();
	 this.tb_idle_reason = new System.Windows.Forms.TextBox();
	 this.label17 = new System.Windows.Forms.Label();
	 this.label11 = new System.Windows.Forms.Label();
	 this.tb_motivo_cura = new System.Windows.Forms.TextBox();
	 this.tb_ultima_cura = new System.Windows.Forms.TextBox();
	 this.debug_HOJ = new System.Windows.Forms.CheckBox();
	 this.debug_potion = new System.Windows.Forms.CheckBox();
	 this.debug_LOH = new System.Windows.Forms.CheckBox();
	 this.debug_dprot = new System.Windows.Forms.CheckBox();
	 this.debug_BOP = new System.Windows.Forms.CheckBox();
	 this.debug_forbearance = new System.Windows.Forms.CheckBox();
	 this.debug_gcd = new System.Windows.Forms.CheckBox();
	 this.cb_atualiza_mapa = new System.Windows.Forms.CheckBox();
	 this.button13 = new System.Windows.Forms.Button();
	 this.button1 = new System.Windows.Forms.Button();
	 this.tb_mapname = new System.Windows.Forms.TextBox();
	 this.bt_loadWP = new System.Windows.Forms.Button();
	 this.bt_anda = new System.Windows.Forms.Button();
	 this.tb_y = new System.Windows.Forms.TextBox();
	 this.tb_x = new System.Windows.Forms.TextBox();
	 this.tb_spd = new System.Windows.Forms.TextBox();
	 this.tb_yaw = new System.Windows.Forms.TextBox();
	 this.bt_getstats = new System.Windows.Forms.Button();
	 this.bt_debug1 = new System.Windows.Forms.Button();
	 this.tb_debug1 = new System.Windows.Forms.TextBox();
	 this.tb_debug2 = new System.Windows.Forms.TextBox();
	 this.tb_debug3 = new System.Windows.Forms.TextBox();
	 this.bt_debug2 = new System.Windows.Forms.Button();
	 this.tb_debug4 = new System.Windows.Forms.TextBox();
	 this.cb_debug = new System.Windows.Forms.CheckBox();
	 this.cb_log = new System.Windows.Forms.CheckBox();
	 this.bt_save_cfg = new System.Windows.Forms.Button();
	 this.tb_debug = new System.Windows.Forms.TextBox();
	 this.label14 = new System.Windows.Forms.Label();
	 this.label15 = new System.Windows.Forms.Label();
	 this.lb_delta = new System.Windows.Forms.Label();
	 this.lb_yaw = new System.Windows.Forms.Label();
	 this.button12 = new System.Windows.Forms.Button();
	 this.tb_loot_tries = new System.Windows.Forms.TextBox();
	 this.label22 = new System.Windows.Forms.Label();
	 this.button16 = new System.Windows.Forms.Button();
	 this.button9 = new System.Windows.Forms.Button();
	 this.button19 = new System.Windows.Forms.Button();
	 this.tb_rogue_eat_at = new System.Windows.Forms.TextBox();
	 this.label25 = new System.Windows.Forms.Label();
	 this.tab_nav.SuspendLayout();
	 this.tabPage1.SuspendLayout();
	 ((System.ComponentModel.ISupportInitialize)(this.pb_minimap)).BeginInit();
	 this.tabPage2.SuspendLayout();
	 ((System.ComponentModel.ISupportInitialize)(this.pb_map)).BeginInit();
	 this.tab_buffs.SuspendLayout();
	 this.Paladin.SuspendLayout();
	 this.tabPage4.SuspendLayout();
	 this.pan_tar.SuspendLayout();
	 this.pan_me.SuspendLayout();
	 this.tabPage3.SuspendLayout();
	 this.SuspendLayout();
	 // 
	 // tab_nav
	 // 
	 this.tab_nav.Controls.Add(this.tabPage1);
	 this.tab_nav.Controls.Add(this.tabPage2);
	 this.tab_nav.Controls.Add(this.tabPage3);
	 this.tab_nav.Location = new System.Drawing.Point(115, 4);
	 this.tab_nav.Name = "tab_nav";
	 this.tab_nav.SelectedIndex = 0;
	 this.tab_nav.Size = new System.Drawing.Size(705, 466);
	 this.tab_nav.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
	 this.tab_nav.TabIndex = 0;
	 // 
	 // tabPage1
	 // 
	 this.tabPage1.BackColor = System.Drawing.Color.DimGray;
	 this.tabPage1.Controls.Add(this.tb_mobs);
	 this.tabPage1.Controls.Add(this.tb_timer_hours);
	 this.tabPage1.Controls.Add(this.button18);
	 this.tabPage1.Controls.Add(this.button17);
	 this.tabPage1.Controls.Add(this.cb_assist_tank);
	 this.tabPage1.Controls.Add(this.cb_HS_timer);
	 this.tabPage1.Controls.Add(this.hs_min_left);
	 this.tabPage1.Controls.Add(this.button15);
	 this.tabPage1.Controls.Add(this.find_dots);
	 this.tabPage1.Controls.Add(this.button14);
	 this.tabPage1.Controls.Add(this.button10);
	 this.tabPage1.Controls.Add(this.tbraio);
	 this.tabPage1.Controls.Add(this.tby);
	 this.tabPage1.Controls.Add(this.tbx);
	 this.tabPage1.Controls.Add(this.pb_minimap);
	 this.tabPage1.Controls.Add(this.label18);
	 this.tabPage1.Controls.Add(this.tb_back_limiar);
	 this.tabPage1.Controls.Add(this.label9);
	 this.tabPage1.Controls.Add(this.label8);
	 this.tabPage1.Controls.Add(this.tbavdecay);
	 this.tabPage1.Controls.Add(this.tbdecay);
	 this.tabPage1.Controls.Add(this.rtbcursor);
	 this.tabPage1.Controls.Add(this.button11);
	 this.tabPage1.Controls.Add(this.button7);
	 this.tabPage1.Controls.Add(this.bt_saveWPas);
	 this.tabPage1.Controls.Add(this.tb_filename);
	 this.tabPage1.Controls.Add(this.bt_debug3);
	 this.tabPage1.Controls.Add(this.button8);
	 this.tabPage1.Controls.Add(this.cb_anda);
	 this.tabPage1.Controls.Add(this.bt_saveWP);
	 this.tabPage1.Controls.Add(this.cb_nostop);
	 this.tabPage1.Controls.Add(this.button6);
	 this.tabPage1.Controls.Add(this.tb_WP_distance);
	 this.tabPage1.Controls.Add(this.button5);
	 this.tabPage1.Controls.Add(this.cb_round);
	 this.tabPage1.Controls.Add(this.button4);
	 this.tabPage1.Controls.Add(this.lb_log);
	 this.tabPage1.Controls.Add(this.button3);
	 this.tabPage1.Controls.Add(this.button2);
	 this.tabPage1.Controls.Add(this.lbwp);
	 this.tabPage1.Controls.Add(this.bt_onoff);
	 this.tabPage1.Location = new System.Drawing.Point(4, 22);
	 this.tabPage1.Name = "tabPage1";
	 this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
	 this.tabPage1.Size = new System.Drawing.Size(697, 440);
	 this.tabPage1.TabIndex = 0;
	 this.tabPage1.Text = "Nav";
	 // 
	 // tb_mobs
	 // 
	 this.tb_mobs.Location = new System.Drawing.Point(537, 103);
	 this.tb_mobs.Name = "tb_mobs";
	 this.tb_mobs.Size = new System.Drawing.Size(100, 20);
	 this.tb_mobs.TabIndex = 56;
	 // 
	 // tb_timer_hours
	 // 
	 this.tb_timer_hours.Location = new System.Drawing.Point(8, 405);
	 this.tb_timer_hours.Name = "tb_timer_hours";
	 this.tb_timer_hours.Size = new System.Drawing.Size(23, 20);
	 this.tb_timer_hours.TabIndex = 55;
	 this.tb_timer_hours.Text = "6";
	 // 
	 // button18
	 // 
	 this.button18.Location = new System.Drawing.Point(514, 233);
	 this.button18.Name = "button18";
	 this.button18.Size = new System.Drawing.Size(75, 23);
	 this.button18.TabIndex = 54;
	 this.button18.Text = "Debug B";
	 this.button18.UseVisualStyleBackColor = true;
	 this.button18.Click += new System.EventHandler(this.button18_Click);
	 // 
	 // button17
	 // 
	 this.button17.Location = new System.Drawing.Point(514, 192);
	 this.button17.Name = "button17";
	 this.button17.Size = new System.Drawing.Size(90, 39);
	 this.button17.TabIndex = 53;
	 this.button17.Text = "Debug";
	 this.button17.UseVisualStyleBackColor = true;
	 this.button17.Click += new System.EventHandler(this.button17_Click);
	 // 
	 // cb_assist_tank
	 // 
	 this.cb_assist_tank.AutoSize = true;
	 this.cb_assist_tank.Location = new System.Drawing.Point(113, 91);
	 this.cb_assist_tank.Name = "cb_assist_tank";
	 this.cb_assist_tank.Size = new System.Drawing.Size(77, 17);
	 this.cb_assist_tank.TabIndex = 52;
	 this.cb_assist_tank.Text = "Asisst tank";
	 this.cb_assist_tank.UseVisualStyleBackColor = true;
	 // 
	 // cb_HS_timer
	 // 
	 this.cb_HS_timer.AutoSize = true;
	 this.cb_HS_timer.Checked = true;
	 this.cb_HS_timer.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_HS_timer.Location = new System.Drawing.Point(8, 380);
	 this.cb_HS_timer.Name = "cb_HS_timer";
	 this.cb_HS_timer.Size = new System.Drawing.Size(87, 17);
	 this.cb_HS_timer.TabIndex = 51;
	 this.cb_HS_timer.Text = "Timer (hours)";
	 this.cb_HS_timer.UseVisualStyleBackColor = true;
	 // 
	 // hs_min_left
	 // 
	 this.hs_min_left.Location = new System.Drawing.Point(37, 405);
	 this.hs_min_left.Name = "hs_min_left";
	 this.hs_min_left.Size = new System.Drawing.Size(87, 20);
	 this.hs_min_left.TabIndex = 50;
	 this.hs_min_left.Text = "300";
	 // 
	 // button15
	 // 
	 this.button15.Location = new System.Drawing.Point(112, 256);
	 this.button15.Name = "button15";
	 this.button15.Size = new System.Drawing.Size(75, 23);
	 this.button15.TabIndex = 49;
	 this.button15.Text = "Andaplanta";
	 this.button15.UseVisualStyleBackColor = true;
	 this.button15.Click += new System.EventHandler(this.button15_Click);
	 // 
	 // find_dots
	 // 
	 this.find_dots.Location = new System.Drawing.Point(186, 254);
	 this.find_dots.Name = "find_dots";
	 this.find_dots.Size = new System.Drawing.Size(75, 23);
	 this.find_dots.TabIndex = 46;
	 this.find_dots.Text = "Acha Planta";
	 this.find_dots.UseVisualStyleBackColor = true;
	 this.find_dots.Click += new System.EventHandler(this.find_dots_Click);
	 // 
	 // button14
	 // 
	 this.button14.Location = new System.Drawing.Point(204, 208);
	 this.button14.Name = "button14";
	 this.button14.Size = new System.Drawing.Size(75, 23);
	 this.button14.TabIndex = 45;
	 this.button14.Text = "Loop";
	 this.button14.UseVisualStyleBackColor = true;
	 this.button14.Click += new System.EventHandler(this.button14_Click);
	 // 
	 // button10
	 // 
	 this.button10.Location = new System.Drawing.Point(419, 254);
	 this.button10.Name = "button10";
	 this.button10.Size = new System.Drawing.Size(75, 23);
	 this.button10.TabIndex = 44;
	 this.button10.Text = "button10";
	 this.button10.UseVisualStyleBackColor = true;
	 this.button10.Click += new System.EventHandler(this.button10_Click);
	 // 
	 // tbraio
	 // 
	 this.tbraio.Location = new System.Drawing.Point(419, 9);
	 this.tbraio.Name = "tbraio";
	 this.tbraio.Size = new System.Drawing.Size(52, 20);
	 this.tbraio.TabIndex = 43;
	 this.tbraio.Text = "163";
	 // 
	 // tby
	 // 
	 this.tby.Location = new System.Drawing.Point(365, 10);
	 this.tby.Name = "tby";
	 this.tby.Size = new System.Drawing.Size(47, 20);
	 this.tby.TabIndex = 42;
	 this.tby.Text = "150";
	 // 
	 // tbx
	 // 
	 this.tbx.Location = new System.Drawing.Point(308, 11);
	 this.tbx.Name = "tbx";
	 this.tbx.Size = new System.Drawing.Size(51, 20);
	 this.tbx.TabIndex = 41;
	 this.tbx.Text = "150";
	 // 
	 // pb_minimap
	 // 
	 this.pb_minimap.Location = new System.Drawing.Point(285, 49);
	 this.pb_minimap.Name = "pb_minimap";
	 this.pb_minimap.Size = new System.Drawing.Size(200, 200);
	 this.pb_minimap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
	 this.pb_minimap.TabIndex = 40;
	 this.pb_minimap.TabStop = false;
	 this.pb_minimap.Click += new System.EventHandler(this.pb_minimap_Click);
	 this.pb_minimap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pb_minimap_MouseClick);
	 // 
	 // label18
	 // 
	 this.label18.AutoSize = true;
	 this.label18.Location = new System.Drawing.Point(553, 58);
	 this.label18.Name = "label18";
	 this.label18.Size = new System.Drawing.Size(51, 13);
	 this.label18.TabIndex = 39;
	 this.label18.Text = "BP Limiar";
	 // 
	 // tb_back_limiar
	 // 
	 this.tb_back_limiar.Location = new System.Drawing.Point(619, 55);
	 this.tb_back_limiar.Name = "tb_back_limiar";
	 this.tb_back_limiar.Size = new System.Drawing.Size(54, 20);
	 this.tb_back_limiar.TabIndex = 38;
	 this.tb_back_limiar.Text = "100";
	 // 
	 // label9
	 // 
	 this.label9.AutoSize = true;
	 this.label9.Location = new System.Drawing.Point(534, 6);
	 this.label9.Name = "label9";
	 this.label9.Size = new System.Drawing.Size(47, 13);
	 this.label9.TabIndex = 37;
	 this.label9.Text = "Average";
	 // 
	 // label8
	 // 
	 this.label8.AutoSize = true;
	 this.label8.Location = new System.Drawing.Point(466, 6);
	 this.label8.Name = "label8";
	 this.label8.Size = new System.Drawing.Size(69, 13);
	 this.label8.TabIndex = 36;
	 this.label8.Text = "Decay HP/m";
	 // 
	 // tbavdecay
	 // 
	 this.tbavdecay.Location = new System.Drawing.Point(619, 29);
	 this.tbavdecay.Name = "tbavdecay";
	 this.tbavdecay.Size = new System.Drawing.Size(54, 20);
	 this.tbavdecay.TabIndex = 35;
	 this.tbavdecay.Text = "0";
	 // 
	 // tbdecay
	 // 
	 this.tbdecay.Location = new System.Drawing.Point(553, 29);
	 this.tbdecay.Name = "tbdecay";
	 this.tbdecay.Size = new System.Drawing.Size(60, 20);
	 this.tbdecay.TabIndex = 34;
	 this.tbdecay.Text = "0";
	 // 
	 // rtbcursor
	 // 
	 this.rtbcursor.Location = new System.Drawing.Point(112, 237);
	 this.rtbcursor.Name = "rtbcursor";
	 this.rtbcursor.Size = new System.Drawing.Size(100, 20);
	 this.rtbcursor.TabIndex = 32;
	 // 
	 // button11
	 // 
	 this.button11.Location = new System.Drawing.Point(113, 36);
	 this.button11.Name = "button11";
	 this.button11.Size = new System.Drawing.Size(83, 23);
	 this.button11.TabIndex = 31;
	 this.button11.Text = "Remove WP";
	 this.button11.UseVisualStyleBackColor = true;
	 this.button11.Click += new System.EventHandler(this.button11_Click);
	 // 
	 // button7
	 // 
	 this.button7.Location = new System.Drawing.Point(342, 254);
	 this.button7.Name = "button7";
	 this.button7.Size = new System.Drawing.Size(79, 23);
	 this.button7.TabIndex = 20;
	 this.button7.Text = "Acha Flexa";
	 this.button7.UseVisualStyleBackColor = true;
	 this.button7.Click += new System.EventHandler(this.button7_Click);
	 // 
	 // bt_saveWPas
	 // 
	 this.bt_saveWPas.Location = new System.Drawing.Point(202, 61);
	 this.bt_saveWPas.Name = "bt_saveWPas";
	 this.bt_saveWPas.Size = new System.Drawing.Size(75, 23);
	 this.bt_saveWPas.TabIndex = 27;
	 this.bt_saveWPas.Text = "Save As";
	 this.bt_saveWPas.UseVisualStyleBackColor = true;
	 this.bt_saveWPas.Click += new System.EventHandler(this.bt_saveWPas_Click);
	 // 
	 // tb_filename
	 // 
	 this.tb_filename.Location = new System.Drawing.Point(202, 9);
	 this.tb_filename.Name = "tb_filename";
	 this.tb_filename.Size = new System.Drawing.Size(100, 20);
	 this.tb_filename.TabIndex = 25;
	 this.tb_filename.Text = "waypoints.txt";
	 // 
	 // bt_debug3
	 // 
	 this.bt_debug3.Location = new System.Drawing.Point(261, 254);
	 this.bt_debug3.Name = "bt_debug3";
	 this.bt_debug3.Size = new System.Drawing.Size(75, 23);
	 this.bt_debug3.TabIndex = 17;
	 this.bt_debug3.Text = "Debug";
	 this.bt_debug3.UseVisualStyleBackColor = true;
	 this.bt_debug3.Click += new System.EventHandler(this.bt_debug3_Click);
	 // 
	 // button8
	 // 
	 this.button8.Location = new System.Drawing.Point(638, 189);
	 this.button8.Name = "button8";
	 this.button8.Size = new System.Drawing.Size(45, 23);
	 this.button8.TabIndex = 23;
	 this.button8.Text = "Clear";
	 this.button8.UseVisualStyleBackColor = true;
	 this.button8.Click += new System.EventHandler(this.button8_Click);
	 // 
	 // cb_anda
	 // 
	 this.cb_anda.AutoSize = true;
	 this.cb_anda.Checked = true;
	 this.cb_anda.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_anda.Location = new System.Drawing.Point(592, 6);
	 this.cb_anda.Name = "cb_anda";
	 this.cb_anda.Size = new System.Drawing.Size(54, 17);
	 this.cb_anda.TabIndex = 22;
	 this.cb_anda.Text = "Andar";
	 this.cb_anda.UseVisualStyleBackColor = true;
	 this.cb_anda.CheckedChanged += new System.EventHandler(this.cb_anda_CheckedChanged);
	 // 
	 // bt_saveWP
	 // 
	 this.bt_saveWP.Location = new System.Drawing.Point(201, 36);
	 this.bt_saveWP.Name = "bt_saveWP";
	 this.bt_saveWP.Size = new System.Drawing.Size(75, 23);
	 this.bt_saveWP.TabIndex = 13;
	 this.bt_saveWP.Text = "Save List";
	 this.bt_saveWP.UseVisualStyleBackColor = true;
	 this.bt_saveWP.Click += new System.EventHandler(this.bt_saveWP_Click);
	 // 
	 // cb_nostop
	 // 
	 this.cb_nostop.AutoSize = true;
	 this.cb_nostop.Checked = true;
	 this.cb_nostop.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_nostop.Location = new System.Drawing.Point(186, 125);
	 this.cb_nostop.Name = "cb_nostop";
	 this.cb_nostop.Size = new System.Drawing.Size(62, 17);
	 this.cb_nostop.TabIndex = 12;
	 this.cb_nostop.Text = "Forever";
	 this.cb_nostop.UseVisualStyleBackColor = true;
	 // 
	 // button6
	 // 
	 this.button6.Location = new System.Drawing.Point(113, 179);
	 this.button6.Name = "button6";
	 this.button6.Size = new System.Drawing.Size(75, 23);
	 this.button6.TabIndex = 11;
	 this.button6.Text = "Clear";
	 this.button6.UseVisualStyleBackColor = true;
	 this.button6.Click += new System.EventHandler(this.button6_Click);
	 // 
	 // tb_WP_distance
	 // 
	 this.tb_WP_distance.Location = new System.Drawing.Point(230, 172);
	 this.tb_WP_distance.Name = "tb_WP_distance";
	 this.tb_WP_distance.Size = new System.Drawing.Size(31, 20);
	 this.tb_WP_distance.TabIndex = 10;
	 this.tb_WP_distance.Text = "150";
	 // 
	 // button5
	 // 
	 this.button5.Location = new System.Drawing.Point(201, 148);
	 this.button5.Name = "button5";
	 this.button5.Size = new System.Drawing.Size(75, 23);
	 this.button5.TabIndex = 9;
	 this.button5.Text = "Record";
	 this.button5.UseVisualStyleBackColor = true;
	 this.button5.Click += new System.EventHandler(this.button5_Click);
	 // 
	 // cb_round
	 // 
	 this.cb_round.AutoSize = true;
	 this.cb_round.Checked = true;
	 this.cb_round.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_round.Location = new System.Drawing.Point(113, 125);
	 this.cb_round.Name = "cb_round";
	 this.cb_round.Size = new System.Drawing.Size(67, 17);
	 this.cb_round.TabIndex = 8;
	 this.cb_round.Text = "Circular?";
	 this.cb_round.UseVisualStyleBackColor = true;
	 // 
	 // button4
	 // 
	 this.button4.Location = new System.Drawing.Point(619, 257);
	 this.button4.Name = "button4";
	 this.button4.Size = new System.Drawing.Size(59, 23);
	 this.button4.TabIndex = 7;
	 this.button4.Text = "Copy";
	 this.button4.UseVisualStyleBackColor = true;
	 this.button4.Click += new System.EventHandler(this.button4_Click);
	 // 
	 // lb_log
	 // 
	 this.lb_log.BackColor = System.Drawing.SystemColors.ScrollBar;
	 this.lb_log.Location = new System.Drawing.Point(131, 286);
	 this.lb_log.Multiline = true;
	 this.lb_log.Name = "lb_log";
	 this.lb_log.Size = new System.Drawing.Size(542, 148);
	 this.lb_log.TabIndex = 6;
	 // 
	 // button3
	 // 
	 this.button3.Location = new System.Drawing.Point(113, 61);
	 this.button3.Name = "button3";
	 this.button3.Size = new System.Drawing.Size(83, 23);
	 this.button3.TabIndex = 4;
	 this.button3.Text = "Goto WP";
	 this.button3.UseVisualStyleBackColor = true;
	 this.button3.Click += new System.EventHandler(this.button3_Click);
	 // 
	 // button2
	 // 
	 this.button2.Location = new System.Drawing.Point(112, 9);
	 this.button2.Name = "button2";
	 this.button2.Size = new System.Drawing.Size(83, 23);
	 this.button2.TabIndex = 2;
	 this.button2.Text = "Add WP";
	 this.button2.UseVisualStyleBackColor = true;
	 this.button2.Click += new System.EventHandler(this.button2_Click);
	 // 
	 // lbwp
	 // 
	 this.lbwp.BackColor = System.Drawing.SystemColors.ScrollBar;
	 this.lbwp.FormattingEnabled = true;
	 this.lbwp.Location = new System.Drawing.Point(8, 40);
	 this.lbwp.Name = "lbwp";
	 this.lbwp.Size = new System.Drawing.Size(98, 316);
	 this.lbwp.TabIndex = 1;
	 // 
	 // bt_onoff
	 // 
	 this.bt_onoff.Location = new System.Drawing.Point(113, 208);
	 this.bt_onoff.Name = "bt_onoff";
	 this.bt_onoff.Size = new System.Drawing.Size(75, 23);
	 this.bt_onoff.TabIndex = 0;
	 this.bt_onoff.Text = "Stop";
	 this.bt_onoff.UseVisualStyleBackColor = true;
	 this.bt_onoff.Click += new System.EventHandler(this.button1_Click);
	 // 
	 // tabPage2
	 // 
	 this.tabPage2.BackColor = System.Drawing.Color.Gray;
	 this.tabPage2.Controls.Add(this.cb_scan_elite);
	 this.tabPage2.Controls.Add(this.cb_elite_patrol);
	 this.tabPage2.Controls.Add(this.cb_rare_patrol);
	 this.tabPage2.Controls.Add(this.tb_wait_patrol);
	 this.tabPage2.Controls.Add(this.cb_humanoid_patrol);
	 this.tabPage2.Controls.Add(this.cb_giant_patrol);
	 this.tabPage2.Controls.Add(this.cb_wrong_gira);
	 this.tabPage2.Controls.Add(this.cb_loot_cloth);
	 this.tabPage2.Controls.Add(this.cb_hearth_bagfull);
	 this.tabPage2.Controls.Add(this.cb_nohumanoid);
	 this.tabPage2.Controls.Add(this.pb_map);
	 this.tabPage2.Controls.Add(this.cb_nodragonkin);
	 this.tabPage2.Controls.Add(this.label21);
	 this.tabPage2.Controls.Add(this.tb_pullcap);
	 this.tabPage2.Controls.Add(this.lb_map_zoom);
	 this.tabPage2.Controls.Add(this.tb_map_zoom);
	 this.tabPage2.Controls.Add(this.tab_buffs);
	 this.tabPage2.Controls.Add(this.pan_tar);
	 this.tabPage2.Controls.Add(this.pan_me);
	 this.tabPage2.Location = new System.Drawing.Point(4, 22);
	 this.tabPage2.Name = "tabPage2";
	 this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
	 this.tabPage2.Size = new System.Drawing.Size(697, 440);
	 this.tabPage2.TabIndex = 1;
	 this.tabPage2.Text = "Combat";
	 // 
	 // cb_scan_elite
	 // 
	 this.cb_scan_elite.AutoSize = true;
	 this.cb_scan_elite.Location = new System.Drawing.Point(4, 347);
	 this.cb_scan_elite.Name = "cb_scan_elite";
	 this.cb_scan_elite.Size = new System.Drawing.Size(79, 17);
	 this.cb_scan_elite.TabIndex = 71;
	 this.cb_scan_elite.Text = "Scan Elites";
	 this.cb_scan_elite.UseVisualStyleBackColor = true;
	 // 
	 // cb_elite_patrol
	 // 
	 this.cb_elite_patrol.AutoSize = true;
	 this.cb_elite_patrol.Checked = true;
	 this.cb_elite_patrol.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_elite_patrol.Location = new System.Drawing.Point(6, 321);
	 this.cb_elite_patrol.Name = "cb_elite_patrol";
	 this.cb_elite_patrol.Size = new System.Drawing.Size(51, 17);
	 this.cb_elite_patrol.TabIndex = 70;
	 this.cb_elite_patrol.Text = "Elites";
	 this.cb_elite_patrol.UseVisualStyleBackColor = true;
	 // 
	 // cb_rare_patrol
	 // 
	 this.cb_rare_patrol.AutoSize = true;
	 this.cb_rare_patrol.Checked = true;
	 this.cb_rare_patrol.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_rare_patrol.Location = new System.Drawing.Point(6, 300);
	 this.cb_rare_patrol.Name = "cb_rare_patrol";
	 this.cb_rare_patrol.Size = new System.Drawing.Size(54, 17);
	 this.cb_rare_patrol.TabIndex = 69;
	 this.cb_rare_patrol.Text = "Raros";
	 this.cb_rare_patrol.UseVisualStyleBackColor = true;
	 // 
	 // tb_wait_patrol
	 // 
	 this.tb_wait_patrol.Location = new System.Drawing.Point(71, 240);
	 this.tb_wait_patrol.Name = "tb_wait_patrol";
	 this.tb_wait_patrol.Size = new System.Drawing.Size(20, 20);
	 this.tb_wait_patrol.TabIndex = 68;
	 this.tb_wait_patrol.Text = "60";
	 // 
	 // cb_humanoid_patrol
	 // 
	 this.cb_humanoid_patrol.AutoSize = true;
	 this.cb_humanoid_patrol.Location = new System.Drawing.Point(6, 283);
	 this.cb_humanoid_patrol.Name = "cb_humanoid_patrol";
	 this.cb_humanoid_patrol.Size = new System.Drawing.Size(74, 17);
	 this.cb_humanoid_patrol.TabIndex = 67;
	 this.cb_humanoid_patrol.Text = "Humanoid";
	 this.cb_humanoid_patrol.UseVisualStyleBackColor = true;
	 // 
	 // cb_giant_patrol
	 // 
	 this.cb_giant_patrol.AutoSize = true;
	 this.cb_giant_patrol.Checked = true;
	 this.cb_giant_patrol.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_giant_patrol.Location = new System.Drawing.Point(6, 269);
	 this.cb_giant_patrol.Name = "cb_giant_patrol";
	 this.cb_giant_patrol.Size = new System.Drawing.Size(51, 17);
	 this.cb_giant_patrol.TabIndex = 66;
	 this.cb_giant_patrol.Text = "Giant";
	 this.cb_giant_patrol.UseVisualStyleBackColor = true;
	 // 
	 // cb_wrong_gira
	 // 
	 this.cb_wrong_gira.AutoSize = true;
	 this.cb_wrong_gira.Checked = true;
	 this.cb_wrong_gira.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_wrong_gira.Location = new System.Drawing.Point(532, 9);
	 this.cb_wrong_gira.Name = "cb_wrong_gira";
	 this.cb_wrong_gira.Size = new System.Drawing.Size(122, 17);
	 this.cb_wrong_gira.TabIndex = 65;
	 this.cb_wrong_gira.Text = "Gira com Wrongway";
	 this.cb_wrong_gira.UseVisualStyleBackColor = true;
	 // 
	 // cb_loot_cloth
	 // 
	 this.cb_loot_cloth.AutoSize = true;
	 this.cb_loot_cloth.Location = new System.Drawing.Point(283, 3);
	 this.cb_loot_cloth.Name = "cb_loot_cloth";
	 this.cb_loot_cloth.Size = new System.Drawing.Size(143, 17);
	 this.cb_loot_cloth.TabIndex = 63;
	 this.cb_loot_cloth.Text = "Loot with bags full (cloth)";
	 this.cb_loot_cloth.UseVisualStyleBackColor = true;
	 // 
	 // cb_hearth_bagfull
	 // 
	 this.cb_hearth_bagfull.AutoSize = true;
	 this.cb_hearth_bagfull.Location = new System.Drawing.Point(162, 6);
	 this.cb_hearth_bagfull.Name = "cb_hearth_bagfull";
	 this.cb_hearth_bagfull.Size = new System.Drawing.Size(115, 17);
	 this.cb_hearth_bagfull.TabIndex = 62;
	 this.cb_hearth_bagfull.Text = "Hearth on full bags";
	 this.cb_hearth_bagfull.UseVisualStyleBackColor = true;
	 // 
	 // cb_nohumanoid
	 // 
	 this.cb_nohumanoid.AutoSize = true;
	 this.cb_nohumanoid.Checked = true;
	 this.cb_nohumanoid.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_nohumanoid.Location = new System.Drawing.Point(432, 25);
	 this.cb_nohumanoid.Name = "cb_nohumanoid";
	 this.cb_nohumanoid.Size = new System.Drawing.Size(89, 17);
	 this.cb_nohumanoid.TabIndex = 61;
	 this.cb_nohumanoid.Text = "No humanoid";
	 this.cb_nohumanoid.UseVisualStyleBackColor = true;
	 // 
	 // pb_map
	 // 
	 this.pb_map.Location = new System.Drawing.Point(113, 235);
	 this.pb_map.Name = "pb_map";
	 this.pb_map.Size = new System.Drawing.Size(196, 188);
	 this.pb_map.TabIndex = 26;
	 this.pb_map.TabStop = false;
	 // 
	 // cb_nodragonkin
	 // 
	 this.cb_nodragonkin.AutoSize = true;
	 this.cb_nodragonkin.Checked = true;
	 this.cb_nodragonkin.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_nodragonkin.Location = new System.Drawing.Point(432, 9);
	 this.cb_nodragonkin.Name = "cb_nodragonkin";
	 this.cb_nodragonkin.Size = new System.Drawing.Size(92, 17);
	 this.cb_nodragonkin.TabIndex = 60;
	 this.cb_nodragonkin.Text = "No Dragonkin";
	 this.cb_nodragonkin.UseVisualStyleBackColor = true;
	 // 
	 // label21
	 // 
	 this.label21.AutoSize = true;
	 this.label21.Location = new System.Drawing.Point(315, 29);
	 this.label21.Name = "label21";
	 this.label21.Size = new System.Drawing.Size(73, 13);
	 this.label21.TabIndex = 59;
	 this.label21.Text = "Pull level cap:";
	 // 
	 // tb_pullcap
	 // 
	 this.tb_pullcap.Location = new System.Drawing.Point(394, 26);
	 this.tb_pullcap.Name = "tb_pullcap";
	 this.tb_pullcap.Size = new System.Drawing.Size(32, 20);
	 this.tb_pullcap.TabIndex = 58;
	 this.tb_pullcap.Text = "0";
	 // 
	 // lb_map_zoom
	 // 
	 this.lb_map_zoom.AutoSize = true;
	 this.lb_map_zoom.Location = new System.Drawing.Point(3, 243);
	 this.lb_map_zoom.Name = "lb_map_zoom";
	 this.lb_map_zoom.Size = new System.Drawing.Size(66, 13);
	 this.lb_map_zoom.TabIndex = 28;
	 this.lb_map_zoom.Text = "Wait patrols:";
	 // 
	 // tb_map_zoom
	 // 
	 this.tb_map_zoom.Location = new System.Drawing.Point(9, 414);
	 this.tb_map_zoom.Name = "tb_map_zoom";
	 this.tb_map_zoom.Size = new System.Drawing.Size(43, 20);
	 this.tb_map_zoom.TabIndex = 27;
	 this.tb_map_zoom.Text = "20";
	 // 
	 // tab_buffs
	 // 
	 this.tab_buffs.Controls.Add(this.Paladin);
	 this.tab_buffs.Controls.Add(this.tabPage4);
	 this.tab_buffs.Location = new System.Drawing.Point(314, 52);
	 this.tab_buffs.Name = "tab_buffs";
	 this.tab_buffs.SelectedIndex = 0;
	 this.tab_buffs.Size = new System.Drawing.Size(359, 373);
	 this.tab_buffs.TabIndex = 25;
	 // 
	 // Paladin
	 // 
	 this.Paladin.AllowDrop = true;
	 this.Paladin.BackColor = System.Drawing.Color.Gray;
	 this.Paladin.Controls.Add(this.cb_flashheal);
	 this.Paladin.Controls.Add(this.tb_purify_delay);
	 this.Paladin.Controls.Add(this.cb_BOP);
	 this.Paladin.Controls.Add(this.tb_JOTC_lvl);
	 this.Paladin.Controls.Add(this.cb_keep_JOTC);
	 this.Paladin.Controls.Add(this.label13);
	 this.Paladin.Controls.Add(this.tb_disable_BOW);
	 this.Paladin.Controls.Add(this.cb_keep_JOL);
	 this.Paladin.Controls.Add(this.label12);
	 this.Paladin.Controls.Add(this.tb_combatheal);
	 this.Paladin.Controls.Add(this.label10);
	 this.Paladin.Controls.Add(this.tb_pull_mana);
	 this.Paladin.Controls.Add(this.cb_retaura);
	 this.Paladin.Controls.Add(this.cb_devaura);
	 this.Paladin.Controls.Add(this.tb_BOSA_limiar);
	 this.Paladin.Controls.Add(this.tb_SOL_limiar);
	 this.Paladin.Controls.Add(this.tb_mana_min);
	 this.Paladin.Controls.Add(this.cb_savemana);
	 this.Paladin.Controls.Add(this.cb_SOL);
	 this.Paladin.Controls.Add(this.cb_BOSA);
	 this.Paladin.Controls.Add(this.cb_concentration_aura);
	 this.Paladin.Controls.Add(this.cb_use_exorcism);
	 this.Paladin.Controls.Add(this.cb_BOK);
	 this.Paladin.Controls.Add(this.tb_bow_trig);
	 this.Paladin.Controls.Add(this.cb_BOW);
	 this.Paladin.Controls.Add(this.tb_stoneform_at);
	 this.Paladin.Controls.Add(this.cb_loh);
	 this.Paladin.Controls.Add(this.cb_dwarf);
	 this.Paladin.Controls.Add(this.cb_bubble);
	 this.Paladin.Controls.Add(this.cb_purify);
	 this.Paladin.Controls.Add(this.label19);
	 this.Paladin.Controls.Add(this.tb_interrupt_at);
	 this.Paladin.Controls.Add(this.cb_hammer_range);
	 this.Paladin.Controls.Add(this.cb_use_hammer);
	 this.Paladin.Controls.Add(this.label16);
	 this.Paladin.Controls.Add(this.tb_preheal);
	 this.Paladin.Controls.Add(this.cb_judge_range);
	 this.Paladin.Controls.Add(this.cb_judge);
	 this.Paladin.Controls.Add(this.cb_BOM);
	 this.Paladin.Controls.Add(this.cb_SOR);
	 this.Paladin.Location = new System.Drawing.Point(4, 22);
	 this.Paladin.Name = "Paladin";
	 this.Paladin.Padding = new System.Windows.Forms.Padding(3);
	 this.Paladin.Size = new System.Drawing.Size(351, 347);
	 this.Paladin.TabIndex = 0;
	 this.Paladin.Text = "Paladin";
	 // 
	 // cb_flashheal
	 // 
	 this.cb_flashheal.AutoSize = true;
	 this.cb_flashheal.Checked = true;
	 this.cb_flashheal.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_flashheal.Location = new System.Drawing.Point(221, 280);
	 this.cb_flashheal.Name = "cb_flashheal";
	 this.cb_flashheal.Size = new System.Drawing.Size(89, 17);
	 this.cb_flashheal.TabIndex = 85;
	 this.cb_flashheal.Text = "Flash of Light";
	 this.cb_flashheal.UseVisualStyleBackColor = true;
	 // 
	 // tb_purify_delay
	 // 
	 this.tb_purify_delay.Location = new System.Drawing.Point(64, 245);
	 this.tb_purify_delay.Name = "tb_purify_delay";
	 this.tb_purify_delay.Size = new System.Drawing.Size(24, 20);
	 this.tb_purify_delay.TabIndex = 84;
	 this.tb_purify_delay.Text = "6";
	 // 
	 // cb_BOP
	 // 
	 this.cb_BOP.AutoSize = true;
	 this.cb_BOP.Checked = true;
	 this.cb_BOP.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_BOP.Enabled = false;
	 this.cb_BOP.Location = new System.Drawing.Point(221, 268);
	 this.cb_BOP.Name = "cb_BOP";
	 this.cb_BOP.Size = new System.Drawing.Size(48, 17);
	 this.cb_BOP.TabIndex = 83;
	 this.cb_BOP.Text = "BOP";
	 this.cb_BOP.UseVisualStyleBackColor = true;
	 // 
	 // tb_JOTC_lvl
	 // 
	 this.tb_JOTC_lvl.Location = new System.Drawing.Point(314, 319);
	 this.tb_JOTC_lvl.Name = "tb_JOTC_lvl";
	 this.tb_JOTC_lvl.Size = new System.Drawing.Size(18, 20);
	 this.tb_JOTC_lvl.TabIndex = 82;
	 this.tb_JOTC_lvl.Text = "-2";
	 // 
	 // cb_keep_JOTC
	 // 
	 this.cb_keep_JOTC.AutoSize = true;
	 this.cb_keep_JOTC.Location = new System.Drawing.Point(212, 322);
	 this.cb_keep_JOTC.Name = "cb_keep_JOTC";
	 this.cb_keep_JOTC.Size = new System.Drawing.Size(110, 17);
	 this.cb_keep_JOTC.TabIndex = 81;
	 this.cb_keep_JOTC.Text = "Keep JOTC   Lvl: ";
	 this.cb_keep_JOTC.UseVisualStyleBackColor = true;
	 // 
	 // label13
	 // 
	 this.label13.AutoSize = true;
	 this.label13.Location = new System.Drawing.Point(269, 6);
	 this.label13.Name = "label13";
	 this.label13.Size = new System.Drawing.Size(25, 13);
	 this.label13.TabIndex = 80;
	 this.label13.Text = "ate ";
	 // 
	 // tb_disable_BOW
	 // 
	 this.tb_disable_BOW.Location = new System.Drawing.Point(297, 4);
	 this.tb_disable_BOW.Name = "tb_disable_BOW";
	 this.tb_disable_BOW.Size = new System.Drawing.Size(33, 20);
	 this.tb_disable_BOW.TabIndex = 79;
	 this.tb_disable_BOW.Text = "90";
	 // 
	 // cb_keep_JOL
	 // 
	 this.cb_keep_JOL.AutoSize = true;
	 this.cb_keep_JOL.Checked = true;
	 this.cb_keep_JOL.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_keep_JOL.Location = new System.Drawing.Point(214, 299);
	 this.cb_keep_JOL.Name = "cb_keep_JOL";
	 this.cb_keep_JOL.Size = new System.Drawing.Size(73, 17);
	 this.cb_keep_JOL.TabIndex = 78;
	 this.cb_keep_JOL.Text = "Keep JOL";
	 this.cb_keep_JOL.UseVisualStyleBackColor = true;
	 // 
	 // label12
	 // 
	 this.label12.AutoSize = true;
	 this.label12.Location = new System.Drawing.Point(230, 188);
	 this.label12.Name = "label12";
	 this.label12.Size = new System.Drawing.Size(66, 13);
	 this.label12.TabIndex = 76;
	 this.label12.Text = "Combat heal";
	 // 
	 // tb_combatheal
	 // 
	 this.tb_combatheal.Location = new System.Drawing.Point(299, 185);
	 this.tb_combatheal.Name = "tb_combatheal";
	 this.tb_combatheal.Size = new System.Drawing.Size(33, 20);
	 this.tb_combatheal.TabIndex = 75;
	 this.tb_combatheal.Text = "40";
	 // 
	 // label10
	 // 
	 this.label10.AutoSize = true;
	 this.label10.Location = new System.Drawing.Point(230, 51);
	 this.label10.Name = "label10";
	 this.label10.Size = new System.Drawing.Size(53, 13);
	 this.label10.TabIndex = 74;
	 this.label10.Text = "Pull mana";
	 // 
	 // tb_pull_mana
	 // 
	 this.tb_pull_mana.Location = new System.Drawing.Point(312, 52);
	 this.tb_pull_mana.Name = "tb_pull_mana";
	 this.tb_pull_mana.Size = new System.Drawing.Size(33, 20);
	 this.tb_pull_mana.TabIndex = 73;
	 this.tb_pull_mana.Text = "45";
	 // 
	 // cb_retaura
	 // 
	 this.cb_retaura.AutoSize = true;
	 this.cb_retaura.Checked = true;
	 this.cb_retaura.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_retaura.Location = new System.Drawing.Point(7, 184);
	 this.cb_retaura.Name = "cb_retaura";
	 this.cb_retaura.Size = new System.Drawing.Size(68, 17);
	 this.cb_retaura.TabIndex = 72;
	 this.cb_retaura.Text = "Ret Aura";
	 this.cb_retaura.UseVisualStyleBackColor = true;
	 // 
	 // cb_devaura
	 // 
	 this.cb_devaura.AutoSize = true;
	 this.cb_devaura.Checked = true;
	 this.cb_devaura.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_devaura.Location = new System.Drawing.Point(243, 102);
	 this.cb_devaura.Name = "cb_devaura";
	 this.cb_devaura.Size = new System.Drawing.Size(94, 17);
	 this.cb_devaura.TabIndex = 71;
	 this.cb_devaura.Text = "Devotion Aura";
	 this.cb_devaura.UseVisualStyleBackColor = true;
	 // 
	 // tb_BOSA_limiar
	 // 
	 this.tb_BOSA_limiar.Location = new System.Drawing.Point(329, 120);
	 this.tb_BOSA_limiar.Name = "tb_BOSA_limiar";
	 this.tb_BOSA_limiar.Size = new System.Drawing.Size(28, 20);
	 this.tb_BOSA_limiar.TabIndex = 70;
	 this.tb_BOSA_limiar.Text = "70";
	 // 
	 // tb_SOL_limiar
	 // 
	 this.tb_SOL_limiar.Location = new System.Drawing.Point(297, 249);
	 this.tb_SOL_limiar.Name = "tb_SOL_limiar";
	 this.tb_SOL_limiar.Size = new System.Drawing.Size(40, 20);
	 this.tb_SOL_limiar.TabIndex = 69;
	 this.tb_SOL_limiar.Text = "50";
	 // 
	 // tb_mana_min
	 // 
	 this.tb_mana_min.Location = new System.Drawing.Point(315, 29);
	 this.tb_mana_min.Name = "tb_mana_min";
	 this.tb_mana_min.Size = new System.Drawing.Size(30, 20);
	 this.tb_mana_min.TabIndex = 68;
	 this.tb_mana_min.Text = "50";
	 // 
	 // cb_savemana
	 // 
	 this.cb_savemana.AutoSize = true;
	 this.cb_savemana.Checked = true;
	 this.cb_savemana.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_savemana.Location = new System.Drawing.Point(213, 29);
	 this.cb_savemana.Name = "cb_savemana";
	 this.cb_savemana.Size = new System.Drawing.Size(80, 17);
	 this.cb_savemana.TabIndex = 67;
	 this.cb_savemana.Text = "Save mana";
	 this.cb_savemana.UseVisualStyleBackColor = true;
	 // 
	 // cb_SOL
	 // 
	 this.cb_SOL.AutoSize = true;
	 this.cb_SOL.Checked = true;
	 this.cb_SOL.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_SOL.Location = new System.Drawing.Point(221, 252);
	 this.cb_SOL.Name = "cb_SOL";
	 this.cb_SOL.Size = new System.Drawing.Size(72, 17);
	 this.cb_SOL.TabIndex = 66;
	 this.cb_SOL.Text = "Usar SOL";
	 this.cb_SOL.UseVisualStyleBackColor = true;
	 // 
	 // cb_BOSA
	 // 
	 this.cb_BOSA.AutoSize = true;
	 this.cb_BOSA.Location = new System.Drawing.Point(204, 122);
	 this.cb_BOSA.Name = "cb_BOSA";
	 this.cb_BOSA.Size = new System.Drawing.Size(128, 17);
	 this.cb_BOSA.TabIndex = 65;
	 this.cb_BOSA.Text = "Blessing of Sanctuary";
	 this.cb_BOSA.UseVisualStyleBackColor = true;
	 // 
	 // cb_concentration_aura
	 // 
	 this.cb_concentration_aura.AutoSize = true;
	 this.cb_concentration_aura.Checked = true;
	 this.cb_concentration_aura.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_concentration_aura.Location = new System.Drawing.Point(9, 119);
	 this.cb_concentration_aura.Name = "cb_concentration_aura";
	 this.cb_concentration_aura.Size = new System.Drawing.Size(116, 17);
	 this.cb_concentration_aura.TabIndex = 64;
	 this.cb_concentration_aura.Text = "Concentration aura";
	 this.cb_concentration_aura.UseVisualStyleBackColor = true;
	 // 
	 // cb_use_exorcism
	 // 
	 this.cb_use_exorcism.AutoSize = true;
	 this.cb_use_exorcism.Checked = true;
	 this.cb_use_exorcism.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_use_exorcism.Location = new System.Drawing.Point(10, 102);
	 this.cb_use_exorcism.Name = "cb_use_exorcism";
	 this.cb_use_exorcism.Size = new System.Drawing.Size(90, 17);
	 this.cb_use_exorcism.TabIndex = 63;
	 this.cb_use_exorcism.Text = "Use Exorcism";
	 this.cb_use_exorcism.UseVisualStyleBackColor = true;
	 // 
	 // cb_BOK
	 // 
	 this.cb_BOK.AutoSize = true;
	 this.cb_BOK.Checked = true;
	 this.cb_BOK.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_BOK.Location = new System.Drawing.Point(6, 29);
	 this.cb_BOK.Name = "cb_BOK";
	 this.cb_BOK.Size = new System.Drawing.Size(73, 17);
	 this.cb_BOK.TabIndex = 62;
	 this.cb_BOK.Text = "Usar BOK";
	 this.cb_BOK.UseVisualStyleBackColor = true;
	 // 
	 // tb_bow_trig
	 // 
	 this.tb_bow_trig.Location = new System.Drawing.Point(234, 3);
	 this.tb_bow_trig.Name = "tb_bow_trig";
	 this.tb_bow_trig.Size = new System.Drawing.Size(33, 20);
	 this.tb_bow_trig.TabIndex = 61;
	 this.tb_bow_trig.Text = "40";
	 // 
	 // cb_BOW
	 // 
	 this.cb_BOW.AutoSize = true;
	 this.cb_BOW.Checked = true;
	 this.cb_BOW.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_BOW.Location = new System.Drawing.Point(159, 6);
	 this.cb_BOW.Name = "cb_BOW";
	 this.cb_BOW.Size = new System.Drawing.Size(80, 17);
	 this.cb_BOW.TabIndex = 59;
	 this.cb_BOW.Text = "Usar BOW ";
	 this.cb_BOW.UseVisualStyleBackColor = true;
	 // 
	 // tb_stoneform_at
	 // 
	 this.tb_stoneform_at.Location = new System.Drawing.Point(104, 293);
	 this.tb_stoneform_at.Name = "tb_stoneform_at";
	 this.tb_stoneform_at.Size = new System.Drawing.Size(33, 20);
	 this.tb_stoneform_at.TabIndex = 58;
	 this.tb_stoneform_at.Text = "60";
	 // 
	 // cb_loh
	 // 
	 this.cb_loh.AutoSize = true;
	 this.cb_loh.Checked = true;
	 this.cb_loh.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_loh.Location = new System.Drawing.Point(204, 161);
	 this.cb_loh.Name = "cb_loh";
	 this.cb_loh.Size = new System.Drawing.Size(92, 17);
	 this.cb_loh.TabIndex = 56;
	 this.cb_loh.Text = "Lay on Hands";
	 this.cb_loh.UseVisualStyleBackColor = true;
	 // 
	 // cb_dwarf
	 // 
	 this.cb_dwarf.AutoSize = true;
	 this.cb_dwarf.Location = new System.Drawing.Point(10, 296);
	 this.cb_dwarf.Name = "cb_dwarf";
	 this.cb_dwarf.Size = new System.Drawing.Size(74, 17);
	 this.cb_dwarf.TabIndex = 29;
	 this.cb_dwarf.Text = "Stoneform";
	 this.cb_dwarf.UseVisualStyleBackColor = true;
	 // 
	 // cb_bubble
	 // 
	 this.cb_bubble.AutoSize = true;
	 this.cb_bubble.Checked = true;
	 this.cb_bubble.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_bubble.Enabled = false;
	 this.cb_bubble.Location = new System.Drawing.Point(234, 209);
	 this.cb_bubble.Name = "cb_bubble";
	 this.cb_bubble.Size = new System.Drawing.Size(59, 17);
	 this.cb_bubble.TabIndex = 55;
	 this.cb_bubble.Text = "Bubble";
	 this.cb_bubble.UseVisualStyleBackColor = true;
	 // 
	 // cb_purify
	 // 
	 this.cb_purify.AutoSize = true;
	 this.cb_purify.Checked = true;
	 this.cb_purify.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_purify.Location = new System.Drawing.Point(6, 248);
	 this.cb_purify.Name = "cb_purify";
	 this.cb_purify.Size = new System.Drawing.Size(52, 17);
	 this.cb_purify.TabIndex = 54;
	 this.cb_purify.Text = "Purify";
	 this.cb_purify.UseVisualStyleBackColor = true;
	 // 
	 // label19
	 // 
	 this.label19.AutoSize = true;
	 this.label19.Location = new System.Drawing.Point(27, 273);
	 this.label19.Name = "label19";
	 this.label19.Size = new System.Drawing.Size(58, 13);
	 this.label19.TabIndex = 53;
	 this.label19.Text = "Interrupt at";
	 // 
	 // tb_interrupt_at
	 // 
	 this.tb_interrupt_at.Location = new System.Drawing.Point(104, 270);
	 this.tb_interrupt_at.Name = "tb_interrupt_at";
	 this.tb_interrupt_at.Size = new System.Drawing.Size(33, 20);
	 this.tb_interrupt_at.TabIndex = 52;
	 this.tb_interrupt_at.Text = "50";
	 // 
	 // cb_hammer_range
	 // 
	 this.cb_hammer_range.AutoSize = true;
	 this.cb_hammer_range.Checked = true;
	 this.cb_hammer_range.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_hammer_range.Location = new System.Drawing.Point(97, 160);
	 this.cb_hammer_range.Name = "cb_hammer_range";
	 this.cb_hammer_range.Size = new System.Drawing.Size(65, 17);
	 this.cb_hammer_range.TabIndex = 50;
	 this.cb_hammer_range.Text = "In range";
	 this.cb_hammer_range.UseVisualStyleBackColor = true;
	 // 
	 // cb_use_hammer
	 // 
	 this.cb_use_hammer.AutoSize = true;
	 this.cb_use_hammer.Checked = true;
	 this.cb_use_hammer.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_use_hammer.Location = new System.Drawing.Point(10, 161);
	 this.cb_use_hammer.Name = "cb_use_hammer";
	 this.cb_use_hammer.Size = new System.Drawing.Size(65, 17);
	 this.cb_use_hammer.TabIndex = 47;
	 this.cb_use_hammer.Text = "Hammer";
	 this.cb_use_hammer.UseVisualStyleBackColor = true;
	 // 
	 // label16
	 // 
	 this.label16.AutoSize = true;
	 this.label16.Location = new System.Drawing.Point(205, 229);
	 this.label16.Name = "label16";
	 this.label16.Size = new System.Drawing.Size(88, 13);
	 this.label16.TabIndex = 46;
	 this.label16.Text = "Pre-Combat heal ";
	 // 
	 // tb_preheal
	 // 
	 this.tb_preheal.Location = new System.Drawing.Point(299, 226);
	 this.tb_preheal.Name = "tb_preheal";
	 this.tb_preheal.Size = new System.Drawing.Size(33, 20);
	 this.tb_preheal.TabIndex = 45;
	 this.tb_preheal.Text = "70";
	 // 
	 // cb_judge_range
	 // 
	 this.cb_judge_range.AutoSize = true;
	 this.cb_judge_range.Checked = true;
	 this.cb_judge_range.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_judge_range.Location = new System.Drawing.Point(97, 137);
	 this.cb_judge_range.Name = "cb_judge_range";
	 this.cb_judge_range.Size = new System.Drawing.Size(65, 17);
	 this.cb_judge_range.TabIndex = 40;
	 this.cb_judge_range.Text = "In range";
	 this.cb_judge_range.UseVisualStyleBackColor = true;
	 // 
	 // cb_judge
	 // 
	 this.cb_judge.AutoSize = true;
	 this.cb_judge.Checked = true;
	 this.cb_judge.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_judge.Location = new System.Drawing.Point(10, 138);
	 this.cb_judge.Name = "cb_judge";
	 this.cb_judge.Size = new System.Drawing.Size(78, 17);
	 this.cb_judge.TabIndex = 37;
	 this.cb_judge.Text = "Judgement";
	 this.cb_judge.UseVisualStyleBackColor = true;
	 // 
	 // cb_BOM
	 // 
	 this.cb_BOM.AutoSize = true;
	 this.cb_BOM.Checked = true;
	 this.cb_BOM.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_BOM.Location = new System.Drawing.Point(6, 71);
	 this.cb_BOM.Name = "cb_BOM";
	 this.cb_BOM.Size = new System.Drawing.Size(75, 17);
	 this.cb_BOM.TabIndex = 33;
	 this.cb_BOM.Text = "Usar BOM";
	 this.cb_BOM.UseVisualStyleBackColor = true;
	 this.cb_BOM.CheckedChanged += new System.EventHandler(this.cb_BOM_CheckedChanged);
	 // 
	 // cb_SOR
	 // 
	 this.cb_SOR.AutoSize = true;
	 this.cb_SOR.Checked = true;
	 this.cb_SOR.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_SOR.Location = new System.Drawing.Point(7, 8);
	 this.cb_SOR.Name = "cb_SOR";
	 this.cb_SOR.Size = new System.Drawing.Size(74, 17);
	 this.cb_SOR.TabIndex = 0;
	 this.cb_SOR.Text = "Usar SOR";
	 this.cb_SOR.UseVisualStyleBackColor = true;
	 // 
	 // tabPage4
	 // 
	 this.tabPage4.BackColor = System.Drawing.Color.Gray;
	 this.tabPage4.Controls.Add(this.tb_rogue_eat_at);
	 this.tabPage4.Controls.Add(this.label25);
	 this.tabPage4.Controls.Add(this.tb_evis_cp);
	 this.tabPage4.Controls.Add(this.label24);
	 this.tabPage4.Controls.Add(this.label23);
	 this.tabPage4.Controls.Add(this.tb_combos);
	 this.tabPage4.Location = new System.Drawing.Point(4, 22);
	 this.tabPage4.Name = "tabPage4";
	 this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
	 this.tabPage4.Size = new System.Drawing.Size(351, 347);
	 this.tabPage4.TabIndex = 1;
	 this.tabPage4.Text = "Rogue";
	 // 
	 // tb_evis_cp
	 // 
	 this.tb_evis_cp.Location = new System.Drawing.Point(76, 35);
	 this.tb_evis_cp.Name = "tb_evis_cp";
	 this.tb_evis_cp.Size = new System.Drawing.Size(26, 20);
	 this.tb_evis_cp.TabIndex = 3;
	 this.tb_evis_cp.Text = "5";
	 // 
	 // label24
	 // 
	 this.label24.AutoSize = true;
	 this.label24.Location = new System.Drawing.Point(7, 38);
	 this.label24.Name = "label24";
	 this.label24.Size = new System.Drawing.Size(69, 13);
	 this.label24.TabIndex = 2;
	 this.label24.Text = "Eviscerate at";
	 // 
	 // label23
	 // 
	 this.label23.AutoSize = true;
	 this.label23.Location = new System.Drawing.Point(27, 16);
	 this.label23.Name = "label23";
	 this.label23.Size = new System.Drawing.Size(43, 13);
	 this.label23.TabIndex = 1;
	 this.label23.Text = "Combo:";
	 // 
	 // tb_combos
	 // 
	 this.tb_combos.Location = new System.Drawing.Point(76, 13);
	 this.tb_combos.Name = "tb_combos";
	 this.tb_combos.Size = new System.Drawing.Size(22, 20);
	 this.tb_combos.TabIndex = 0;
	 this.tb_combos.Text = "0";
	 // 
	 // pan_tar
	 // 
	 this.pan_tar.Controls.Add(this.cb_killgray);
	 this.pan_tar.Controls.Add(this.textBox1);
	 this.pan_tar.Controls.Add(this.cb_melee);
	 this.pan_tar.Controls.Add(this.label2);
	 this.pan_tar.Controls.Add(this.tb_tarcast);
	 this.pan_tar.Controls.Add(this.pb_tarcast);
	 this.pan_tar.Controls.Add(this.tb_mood);
	 this.pan_tar.Controls.Add(this.label7);
	 this.pan_tar.Controls.Add(this.tb_tarlevel);
	 this.pan_tar.Controls.Add(this.label6);
	 this.pan_tar.Controls.Add(this.tb_tartype);
	 this.pan_tar.Controls.Add(this.tb_tarhp);
	 this.pan_tar.Controls.Add(this.label1);
	 this.pan_tar.Location = new System.Drawing.Point(157, 29);
	 this.pan_tar.Name = "pan_tar";
	 this.pan_tar.Size = new System.Drawing.Size(142, 199);
	 this.pan_tar.TabIndex = 24;
	 // 
	 // cb_killgray
	 // 
	 this.cb_killgray.AutoSize = true;
	 this.cb_killgray.Location = new System.Drawing.Point(88, 54);
	 this.cb_killgray.Name = "cb_killgray";
	 this.cb_killgray.Size = new System.Drawing.Size(54, 17);
	 this.cb_killgray.TabIndex = 40;
	 this.cb_killgray.Text = "Gray?";
	 this.cb_killgray.UseVisualStyleBackColor = true;
	 // 
	 // textBox1
	 // 
	 this.textBox1.Location = new System.Drawing.Point(106, 100);
	 this.textBox1.Name = "textBox1";
	 this.textBox1.Size = new System.Drawing.Size(31, 20);
	 this.textBox1.TabIndex = 23;
	 this.textBox1.Text = "0";
	 // 
	 // cb_melee
	 // 
	 this.cb_melee.AutoSize = true;
	 this.cb_melee.Location = new System.Drawing.Point(45, 103);
	 this.cb_melee.Name = "cb_melee";
	 this.cb_melee.Size = new System.Drawing.Size(55, 17);
	 this.cb_melee.TabIndex = 39;
	 this.cb_melee.Text = "Melee";
	 this.cb_melee.UseVisualStyleBackColor = true;
	 // 
	 // label2
	 // 
	 this.label2.AutoSize = true;
	 this.label2.Location = new System.Drawing.Point(5, 104);
	 this.label2.Name = "label2";
	 this.label2.Size = new System.Drawing.Size(42, 13);
	 this.label2.TabIndex = 38;
	 this.label2.Text = "Range:";
	 // 
	 // tb_tarcast
	 // 
	 this.tb_tarcast.Location = new System.Drawing.Point(1, 77);
	 this.tb_tarcast.Name = "tb_tarcast";
	 this.tb_tarcast.Size = new System.Drawing.Size(28, 20);
	 this.tb_tarcast.TabIndex = 36;
	 // 
	 // pb_tarcast
	 // 
	 this.pb_tarcast.Location = new System.Drawing.Point(37, 74);
	 this.pb_tarcast.Name = "pb_tarcast";
	 this.pb_tarcast.Size = new System.Drawing.Size(100, 23);
	 this.pb_tarcast.TabIndex = 37;
	 // 
	 // tb_mood
	 // 
	 this.tb_mood.Location = new System.Drawing.Point(37, 51);
	 this.tb_mood.Name = "tb_mood";
	 this.tb_mood.Size = new System.Drawing.Size(49, 20);
	 this.tb_mood.TabIndex = 35;
	 // 
	 // label7
	 // 
	 this.label7.AutoSize = true;
	 this.label7.Location = new System.Drawing.Point(0, 56);
	 this.label7.Name = "label7";
	 this.label7.Size = new System.Drawing.Size(37, 13);
	 this.label7.TabIndex = 34;
	 this.label7.Text = "Mood:";
	 // 
	 // tb_tarlevel
	 // 
	 this.tb_tarlevel.Location = new System.Drawing.Point(106, 5);
	 this.tb_tarlevel.Name = "tb_tarlevel";
	 this.tb_tarlevel.Size = new System.Drawing.Size(31, 20);
	 this.tb_tarlevel.TabIndex = 33;
	 // 
	 // label6
	 // 
	 this.label6.AutoSize = true;
	 this.label6.Location = new System.Drawing.Point(2, 8);
	 this.label6.Name = "label6";
	 this.label6.Size = new System.Drawing.Size(31, 13);
	 this.label6.TabIndex = 32;
	 this.label6.Text = "Type";
	 // 
	 // tb_tartype
	 // 
	 this.tb_tartype.Location = new System.Drawing.Point(41, 5);
	 this.tb_tartype.Name = "tb_tartype";
	 this.tb_tartype.Size = new System.Drawing.Size(59, 20);
	 this.tb_tartype.TabIndex = 31;
	 // 
	 // tb_tarhp
	 // 
	 this.tb_tarhp.Location = new System.Drawing.Point(37, 30);
	 this.tb_tarhp.Name = "tb_tarhp";
	 this.tb_tarhp.Size = new System.Drawing.Size(31, 20);
	 this.tb_tarhp.TabIndex = 23;
	 // 
	 // label1
	 // 
	 this.label1.AutoSize = true;
	 this.label1.Location = new System.Drawing.Point(8, 35);
	 this.label1.Name = "label1";
	 this.label1.Size = new System.Drawing.Size(28, 13);
	 this.label1.TabIndex = 0;
	 this.label1.Text = "HP: ";
	 // 
	 // pan_me
	 // 
	 this.pan_me.Controls.Add(this.cb_apagacinza);
	 this.pan_me.Controls.Add(this.cb_pacifist);
	 this.pan_me.Controls.Add(this.cb_skinning);
	 this.pan_me.Controls.Add(this.cb_loot);
	 this.pan_me.Controls.Add(this.cb_herbalism);
	 this.pan_me.Controls.Add(this.cb_pull);
	 this.pan_me.Controls.Add(this.cb_autoattack);
	 this.pan_me.Controls.Add(this.tb_playercast);
	 this.pan_me.Controls.Add(this.pb_playercast);
	 this.pan_me.Controls.Add(this.tb_level);
	 this.pan_me.Controls.Add(this.label5);
	 this.pan_me.Controls.Add(this.cb_combat);
	 this.pan_me.Controls.Add(this.tb_class);
	 this.pan_me.Controls.Add(this.label3);
	 this.pan_me.Controls.Add(this.tb_hp);
	 this.pan_me.Controls.Add(this.label4);
	 this.pan_me.Controls.Add(this.tb_mana);
	 this.pan_me.Location = new System.Drawing.Point(9, 6);
	 this.pan_me.Name = "pan_me";
	 this.pan_me.Size = new System.Drawing.Size(142, 222);
	 this.pan_me.TabIndex = 23;
	 // 
	 // cb_apagacinza
	 // 
	 this.cb_apagacinza.AutoSize = true;
	 this.cb_apagacinza.Checked = true;
	 this.cb_apagacinza.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_apagacinza.Location = new System.Drawing.Point(3, 202);
	 this.cb_apagacinza.Name = "cb_apagacinza";
	 this.cb_apagacinza.Size = new System.Drawing.Size(109, 17);
	 this.cb_apagacinza.TabIndex = 41;
	 this.cb_apagacinza.Text = "Destrói item cinza";
	 this.cb_apagacinza.UseVisualStyleBackColor = true;
	 // 
	 // cb_pacifist
	 // 
	 this.cb_pacifist.AutoSize = true;
	 this.cb_pacifist.Location = new System.Drawing.Point(3, 185);
	 this.cb_pacifist.Name = "cb_pacifist";
	 this.cb_pacifist.Size = new System.Drawing.Size(66, 17);
	 this.cb_pacifist.TabIndex = 48;
	 this.cb_pacifist.Text = "Pacifista";
	 this.cb_pacifist.UseVisualStyleBackColor = true;
	 this.cb_pacifist.CheckedChanged += new System.EventHandler(this.cb_pacifist_CheckedChanged);
	 // 
	 // cb_skinning
	 // 
	 this.cb_skinning.AutoSize = true;
	 this.cb_skinning.Checked = true;
	 this.cb_skinning.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_skinning.Location = new System.Drawing.Point(3, 139);
	 this.cb_skinning.Name = "cb_skinning";
	 this.cb_skinning.Size = new System.Drawing.Size(67, 17);
	 this.cb_skinning.TabIndex = 35;
	 this.cb_skinning.Text = "Skinning";
	 this.cb_skinning.UseVisualStyleBackColor = true;
	 // 
	 // cb_loot
	 // 
	 this.cb_loot.AutoSize = true;
	 this.cb_loot.Checked = true;
	 this.cb_loot.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_loot.Location = new System.Drawing.Point(3, 125);
	 this.cb_loot.Name = "cb_loot";
	 this.cb_loot.Size = new System.Drawing.Size(75, 17);
	 this.cb_loot.TabIndex = 34;
	 this.cb_loot.Text = "Loot mobs";
	 this.cb_loot.UseVisualStyleBackColor = true;
	 // 
	 // cb_herbalism
	 // 
	 this.cb_herbalism.AutoSize = true;
	 this.cb_herbalism.Location = new System.Drawing.Point(3, 162);
	 this.cb_herbalism.Name = "cb_herbalism";
	 this.cb_herbalism.Size = new System.Drawing.Size(72, 17);
	 this.cb_herbalism.TabIndex = 47;
	 this.cb_herbalism.Text = "Herbalism";
	 this.cb_herbalism.UseVisualStyleBackColor = true;
	 // 
	 // cb_pull
	 // 
	 this.cb_pull.AutoSize = true;
	 this.cb_pull.Checked = true;
	 this.cb_pull.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_pull.Location = new System.Drawing.Point(74, 54);
	 this.cb_pull.Name = "cb_pull";
	 this.cb_pull.Size = new System.Drawing.Size(43, 17);
	 this.cb_pull.TabIndex = 33;
	 this.cb_pull.Text = "Pull";
	 this.cb_pull.UseVisualStyleBackColor = true;
	 // 
	 // cb_autoattack
	 // 
	 this.cb_autoattack.AutoSize = true;
	 this.cb_autoattack.Location = new System.Drawing.Point(4, 108);
	 this.cb_autoattack.Name = "cb_autoattack";
	 this.cb_autoattack.Size = new System.Drawing.Size(78, 17);
	 this.cb_autoattack.TabIndex = 32;
	 this.cb_autoattack.Text = "Autoattack";
	 this.cb_autoattack.UseVisualStyleBackColor = true;
	 // 
	 // tb_playercast
	 // 
	 this.tb_playercast.Location = new System.Drawing.Point(3, 81);
	 this.tb_playercast.Name = "tb_playercast";
	 this.tb_playercast.Size = new System.Drawing.Size(28, 20);
	 this.tb_playercast.TabIndex = 23;
	 // 
	 // pb_playercast
	 // 
	 this.pb_playercast.Location = new System.Drawing.Point(39, 78);
	 this.pb_playercast.Name = "pb_playercast";
	 this.pb_playercast.Size = new System.Drawing.Size(100, 23);
	 this.pb_playercast.TabIndex = 31;
	 // 
	 // tb_level
	 // 
	 this.tb_level.Location = new System.Drawing.Point(107, 3);
	 this.tb_level.Name = "tb_level";
	 this.tb_level.Size = new System.Drawing.Size(31, 20);
	 this.tb_level.TabIndex = 30;
	 // 
	 // label5
	 // 
	 this.label5.AutoSize = true;
	 this.label5.Location = new System.Drawing.Point(3, 6);
	 this.label5.Name = "label5";
	 this.label5.Size = new System.Drawing.Size(35, 13);
	 this.label5.TabIndex = 29;
	 this.label5.Text = "Class:";
	 // 
	 // cb_combat
	 // 
	 this.cb_combat.AutoSize = true;
	 this.cb_combat.Checked = true;
	 this.cb_combat.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_combat.Location = new System.Drawing.Point(6, 55);
	 this.cb_combat.Name = "cb_combat";
	 this.cb_combat.Size = new System.Drawing.Size(62, 17);
	 this.cb_combat.TabIndex = 22;
	 this.cb_combat.Text = "Combat";
	 this.cb_combat.UseVisualStyleBackColor = true;
	 // 
	 // tb_class
	 // 
	 this.tb_class.Location = new System.Drawing.Point(42, 3);
	 this.tb_class.Name = "tb_class";
	 this.tb_class.Size = new System.Drawing.Size(59, 20);
	 this.tb_class.TabIndex = 28;
	 // 
	 // label3
	 // 
	 this.label3.AutoSize = true;
	 this.label3.Location = new System.Drawing.Point(65, 32);
	 this.label3.Name = "label3";
	 this.label3.Size = new System.Drawing.Size(37, 13);
	 this.label3.TabIndex = 27;
	 this.label3.Text = "Mana:";
	 // 
	 // tb_hp
	 // 
	 this.tb_hp.Location = new System.Drawing.Point(28, 29);
	 this.tb_hp.Name = "tb_hp";
	 this.tb_hp.Size = new System.Drawing.Size(31, 20);
	 this.tb_hp.TabIndex = 1;
	 // 
	 // label4
	 // 
	 this.label4.AutoSize = true;
	 this.label4.Location = new System.Drawing.Point(3, 32);
	 this.label4.Name = "label4";
	 this.label4.Size = new System.Drawing.Size(28, 13);
	 this.label4.TabIndex = 26;
	 this.label4.Text = "HP: ";
	 // 
	 // tb_mana
	 // 
	 this.tb_mana.Location = new System.Drawing.Point(104, 29);
	 this.tb_mana.Name = "tb_mana";
	 this.tb_mana.Size = new System.Drawing.Size(31, 20);
	 this.tb_mana.TabIndex = 2;
	 // 
	 // tabPage3
	 // 
	 this.tabPage3.BackColor = System.Drawing.Color.DimGray;
	 this.tabPage3.Controls.Add(this.button20);
	 this.tabPage3.Controls.Add(this.cb_logar_cura);
	 this.tabPage3.Controls.Add(this.label20);
	 this.tabPage3.Controls.Add(this.tb_idle_reason);
	 this.tabPage3.Controls.Add(this.label17);
	 this.tabPage3.Controls.Add(this.label11);
	 this.tabPage3.Controls.Add(this.tb_motivo_cura);
	 this.tabPage3.Controls.Add(this.tb_ultima_cura);
	 this.tabPage3.Controls.Add(this.debug_HOJ);
	 this.tabPage3.Controls.Add(this.debug_potion);
	 this.tabPage3.Controls.Add(this.debug_LOH);
	 this.tabPage3.Controls.Add(this.debug_dprot);
	 this.tabPage3.Controls.Add(this.debug_BOP);
	 this.tabPage3.Controls.Add(this.debug_forbearance);
	 this.tabPage3.Controls.Add(this.debug_gcd);
	 this.tabPage3.Controls.Add(this.cb_atualiza_mapa);
	 this.tabPage3.Controls.Add(this.button13);
	 this.tabPage3.Controls.Add(this.button1);
	 this.tabPage3.Controls.Add(this.tb_mapname);
	 this.tabPage3.Location = new System.Drawing.Point(4, 22);
	 this.tabPage3.Name = "tabPage3";
	 this.tabPage3.Size = new System.Drawing.Size(697, 440);
	 this.tabPage3.TabIndex = 2;
	 this.tabPage3.Text = "Mapper";
	 // 
	 // button20
	 // 
	 this.button20.Location = new System.Drawing.Point(2, 281);
	 this.button20.Name = "button20";
	 this.button20.Size = new System.Drawing.Size(75, 23);
	 this.button20.TabIndex = 46;
	 this.button20.Text = "button20";
	 this.button20.UseVisualStyleBackColor = true;
	 this.button20.Click += new System.EventHandler(this.button20_Click);
	 // 
	 // cb_logar_cura
	 // 
	 this.cb_logar_cura.AutoSize = true;
	 this.cb_logar_cura.Location = new System.Drawing.Point(138, 116);
	 this.cb_logar_cura.Name = "cb_logar_cura";
	 this.cb_logar_cura.Size = new System.Drawing.Size(78, 17);
	 this.cb_logar_cura.TabIndex = 45;
	 this.cb_logar_cura.Text = "Logar Cura";
	 this.cb_logar_cura.UseVisualStyleBackColor = true;
	 // 
	 // label20
	 // 
	 this.label20.AutoSize = true;
	 this.label20.Location = new System.Drawing.Point(172, 169);
	 this.label20.Name = "label20";
	 this.label20.Size = new System.Drawing.Size(67, 13);
	 this.label20.TabIndex = 44;
	 this.label20.Text = "Idle Reason:";
	 // 
	 // tb_idle_reason
	 // 
	 this.tb_idle_reason.Location = new System.Drawing.Point(257, 169);
	 this.tb_idle_reason.Name = "tb_idle_reason";
	 this.tb_idle_reason.Size = new System.Drawing.Size(406, 20);
	 this.tb_idle_reason.TabIndex = 43;
	 // 
	 // label17
	 // 
	 this.label17.AutoSize = true;
	 this.label17.Location = new System.Drawing.Point(183, 225);
	 this.label17.Name = "label17";
	 this.label17.Size = new System.Drawing.Size(45, 13);
	 this.label17.TabIndex = 42;
	 this.label17.Text = "Motivo: ";
	 // 
	 // label11
	 // 
	 this.label11.AutoSize = true;
	 this.label11.Location = new System.Drawing.Point(172, 198);
	 this.label11.Name = "label11";
	 this.label11.Size = new System.Drawing.Size(67, 13);
	 this.label11.TabIndex = 41;
	 this.label11.Text = "Ultima Ação:";
	 // 
	 // tb_motivo_cura
	 // 
	 this.tb_motivo_cura.Location = new System.Drawing.Point(257, 222);
	 this.tb_motivo_cura.Multiline = true;
	 this.tb_motivo_cura.Name = "tb_motivo_cura";
	 this.tb_motivo_cura.Size = new System.Drawing.Size(406, 117);
	 this.tb_motivo_cura.TabIndex = 40;
	 // 
	 // tb_ultima_cura
	 // 
	 this.tb_ultima_cura.Location = new System.Drawing.Point(257, 195);
	 this.tb_ultima_cura.Name = "tb_ultima_cura";
	 this.tb_ultima_cura.Size = new System.Drawing.Size(406, 20);
	 this.tb_ultima_cura.TabIndex = 39;
	 // 
	 // debug_HOJ
	 // 
	 this.debug_HOJ.AutoSize = true;
	 this.debug_HOJ.Location = new System.Drawing.Point(493, 87);
	 this.debug_HOJ.Name = "debug_HOJ";
	 this.debug_HOJ.Size = new System.Drawing.Size(47, 17);
	 this.debug_HOJ.TabIndex = 38;
	 this.debug_HOJ.Text = "HOJ";
	 this.debug_HOJ.UseVisualStyleBackColor = true;
	 // 
	 // debug_potion
	 // 
	 this.debug_potion.AutoSize = true;
	 this.debug_potion.Location = new System.Drawing.Point(406, 90);
	 this.debug_potion.Name = "debug_potion";
	 this.debug_potion.Size = new System.Drawing.Size(90, 17);
	 this.debug_potion.TabIndex = 37;
	 this.debug_potion.Text = "Health Potion";
	 this.debug_potion.UseVisualStyleBackColor = true;
	 // 
	 // debug_LOH
	 // 
	 this.debug_LOH.AutoSize = true;
	 this.debug_LOH.Location = new System.Drawing.Point(320, 89);
	 this.debug_LOH.Name = "debug_LOH";
	 this.debug_LOH.Size = new System.Drawing.Size(48, 17);
	 this.debug_LOH.TabIndex = 36;
	 this.debug_LOH.Text = "LOH";
	 this.debug_LOH.UseVisualStyleBackColor = true;
	 // 
	 // debug_dprot
	 // 
	 this.debug_dprot.AutoSize = true;
	 this.debug_dprot.Location = new System.Drawing.Point(234, 135);
	 this.debug_dprot.Name = "debug_dprot";
	 this.debug_dprot.Size = new System.Drawing.Size(78, 17);
	 this.debug_dprot.TabIndex = 35;
	 this.debug_dprot.Text = "Divine Prot";
	 this.debug_dprot.UseVisualStyleBackColor = true;
	 // 
	 // debug_BOP
	 // 
	 this.debug_BOP.AutoSize = true;
	 this.debug_BOP.Location = new System.Drawing.Point(234, 111);
	 this.debug_BOP.Name = "debug_BOP";
	 this.debug_BOP.Size = new System.Drawing.Size(48, 17);
	 this.debug_BOP.TabIndex = 34;
	 this.debug_BOP.Text = "BOP";
	 this.debug_BOP.UseVisualStyleBackColor = true;
	 // 
	 // debug_forbearance
	 // 
	 this.debug_forbearance.AutoSize = true;
	 this.debug_forbearance.Location = new System.Drawing.Point(234, 90);
	 this.debug_forbearance.Name = "debug_forbearance";
	 this.debug_forbearance.Size = new System.Drawing.Size(86, 17);
	 this.debug_forbearance.TabIndex = 33;
	 this.debug_forbearance.Text = "Forbearance";
	 this.debug_forbearance.UseVisualStyleBackColor = true;
	 // 
	 // debug_gcd
	 // 
	 this.debug_gcd.AutoSize = true;
	 this.debug_gcd.Location = new System.Drawing.Point(138, 89);
	 this.debug_gcd.Name = "debug_gcd";
	 this.debug_gcd.Size = new System.Drawing.Size(49, 17);
	 this.debug_gcd.TabIndex = 32;
	 this.debug_gcd.Text = "GCD";
	 this.debug_gcd.UseVisualStyleBackColor = true;
	 // 
	 // cb_atualiza_mapa
	 // 
	 this.cb_atualiza_mapa.AutoSize = true;
	 this.cb_atualiza_mapa.Location = new System.Drawing.Point(257, 17);
	 this.cb_atualiza_mapa.Name = "cb_atualiza_mapa";
	 this.cb_atualiza_mapa.Size = new System.Drawing.Size(92, 17);
	 this.cb_atualiza_mapa.TabIndex = 31;
	 this.cb_atualiza_mapa.Text = "Atualiza mapa";
	 this.cb_atualiza_mapa.UseVisualStyleBackColor = true;
	 // 
	 // button13
	 // 
	 this.button13.Location = new System.Drawing.Point(15, 10);
	 this.button13.Name = "button13";
	 this.button13.Size = new System.Drawing.Size(75, 23);
	 this.button13.TabIndex = 30;
	 this.button13.Text = "Load Map";
	 this.button13.UseVisualStyleBackColor = true;
	 this.button13.Click += new System.EventHandler(this.button13_Click);
	 // 
	 // button1
	 // 
	 this.button1.Location = new System.Drawing.Point(3, 116);
	 this.button1.Name = "button1";
	 this.button1.Size = new System.Drawing.Size(75, 23);
	 this.button1.TabIndex = 29;
	 this.button1.Text = "STOP";
	 this.button1.UseVisualStyleBackColor = true;
	 this.button1.Click += new System.EventHandler(this.button1_Click_2);
	 // 
	 // tb_mapname
	 // 
	 this.tb_mapname.Location = new System.Drawing.Point(96, 12);
	 this.tb_mapname.Name = "tb_mapname";
	 this.tb_mapname.Size = new System.Drawing.Size(134, 20);
	 this.tb_mapname.TabIndex = 28;
	 this.tb_mapname.Text = "HSA";
	 // 
	 // bt_loadWP
	 // 
	 this.bt_loadWP.Location = new System.Drawing.Point(58, 38);
	 this.bt_loadWP.Name = "bt_loadWP";
	 this.bt_loadWP.Size = new System.Drawing.Size(75, 23);
	 this.bt_loadWP.TabIndex = 26;
	 this.bt_loadWP.Text = "Load List";
	 this.bt_loadWP.UseVisualStyleBackColor = true;
	 this.bt_loadWP.Click += new System.EventHandler(this.bt_loadWP_Click);
	 // 
	 // bt_anda
	 // 
	 this.bt_anda.Location = new System.Drawing.Point(2, 38);
	 this.bt_anda.Name = "bt_anda";
	 this.bt_anda.Size = new System.Drawing.Size(61, 23);
	 this.bt_anda.TabIndex = 5;
	 this.bt_anda.Text = "Start WP";
	 this.bt_anda.UseVisualStyleBackColor = true;
	 this.bt_anda.Click += new System.EventHandler(this.button1_Click_1);
	 // 
	 // tb_y
	 // 
	 this.tb_y.Location = new System.Drawing.Point(58, 91);
	 this.tb_y.Name = "tb_y";
	 this.tb_y.Size = new System.Drawing.Size(43, 20);
	 this.tb_y.TabIndex = 4;
	 // 
	 // tb_x
	 // 
	 this.tb_x.Location = new System.Drawing.Point(9, 90);
	 this.tb_x.Name = "tb_x";
	 this.tb_x.Size = new System.Drawing.Size(43, 20);
	 this.tb_x.TabIndex = 3;
	 // 
	 // tb_spd
	 // 
	 this.tb_spd.Location = new System.Drawing.Point(50, 139);
	 this.tb_spd.Name = "tb_spd";
	 this.tb_spd.Size = new System.Drawing.Size(31, 20);
	 this.tb_spd.TabIndex = 6;
	 // 
	 // tb_yaw
	 // 
	 this.tb_yaw.Location = new System.Drawing.Point(49, 113);
	 this.tb_yaw.Name = "tb_yaw";
	 this.tb_yaw.Size = new System.Drawing.Size(31, 20);
	 this.tb_yaw.TabIndex = 7;
	 // 
	 // bt_getstats
	 // 
	 this.bt_getstats.Location = new System.Drawing.Point(2, 166);
	 this.bt_getstats.Name = "bt_getstats";
	 this.bt_getstats.Size = new System.Drawing.Size(75, 23);
	 this.bt_getstats.TabIndex = 8;
	 this.bt_getstats.Text = "Get Stats";
	 this.bt_getstats.UseVisualStyleBackColor = true;
	 this.bt_getstats.Click += new System.EventHandler(this.bt_getstats_Click);
	 // 
	 // bt_debug1
	 // 
	 this.bt_debug1.Location = new System.Drawing.Point(2, 348);
	 this.bt_debug1.Name = "bt_debug1";
	 this.bt_debug1.Size = new System.Drawing.Size(75, 23);
	 this.bt_debug1.TabIndex = 9;
	 this.bt_debug1.Text = "Move to";
	 this.bt_debug1.UseVisualStyleBackColor = true;
	 this.bt_debug1.Click += new System.EventHandler(this.bt_debug1_Click);
	 // 
	 // tb_debug1
	 // 
	 this.tb_debug1.Location = new System.Drawing.Point(2, 377);
	 this.tb_debug1.Name = "tb_debug1";
	 this.tb_debug1.Size = new System.Drawing.Size(50, 20);
	 this.tb_debug1.TabIndex = 11;
	 this.tb_debug1.Text = "5581";
	 // 
	 // tb_debug2
	 // 
	 this.tb_debug2.Location = new System.Drawing.Point(58, 377);
	 this.tb_debug2.Name = "tb_debug2";
	 this.tb_debug2.Size = new System.Drawing.Size(41, 20);
	 this.tb_debug2.TabIndex = 10;
	 this.tb_debug2.Text = "6099";
	 // 
	 // tb_debug3
	 // 
	 this.tb_debug3.Location = new System.Drawing.Point(2, 403);
	 this.tb_debug3.Name = "tb_debug3";
	 this.tb_debug3.Size = new System.Drawing.Size(31, 20);
	 this.tb_debug3.TabIndex = 13;
	 this.tb_debug3.Text = "0";
	 // 
	 // bt_debug2
	 // 
	 this.bt_debug2.Location = new System.Drawing.Point(2, 319);
	 this.bt_debug2.Name = "bt_debug2";
	 this.bt_debug2.Size = new System.Drawing.Size(75, 23);
	 this.bt_debug2.TabIndex = 14;
	 this.bt_debug2.Text = "Get Position";
	 this.bt_debug2.UseVisualStyleBackColor = true;
	 this.bt_debug2.Click += new System.EventHandler(this.bt_debug2_Click);
	 // 
	 // tb_debug4
	 // 
	 this.tb_debug4.Location = new System.Drawing.Point(39, 403);
	 this.tb_debug4.Name = "tb_debug4";
	 this.tb_debug4.Size = new System.Drawing.Size(31, 20);
	 this.tb_debug4.TabIndex = 15;
	 this.tb_debug4.Text = "0";
	 // 
	 // cb_debug
	 // 
	 this.cb_debug.AutoSize = true;
	 this.cb_debug.Checked = true;
	 this.cb_debug.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_debug.Location = new System.Drawing.Point(9, 4);
	 this.cb_debug.Name = "cb_debug";
	 this.cb_debug.Size = new System.Drawing.Size(87, 17);
	 this.cb_debug.TabIndex = 18;
	 this.cb_debug.Text = "Debug mode";
	 this.cb_debug.UseVisualStyleBackColor = true;
	 // 
	 // cb_log
	 // 
	 this.cb_log.AutoSize = true;
	 this.cb_log.Checked = true;
	 this.cb_log.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_log.Location = new System.Drawing.Point(8, 22);
	 this.cb_log.Name = "cb_log";
	 this.cb_log.Size = new System.Drawing.Size(44, 17);
	 this.cb_log.TabIndex = 19;
	 this.cb_log.Text = "Log";
	 this.cb_log.UseVisualStyleBackColor = true;
	 // 
	 // bt_save_cfg
	 // 
	 this.bt_save_cfg.Location = new System.Drawing.Point(2, 195);
	 this.bt_save_cfg.Name = "bt_save_cfg";
	 this.bt_save_cfg.Size = new System.Drawing.Size(79, 23);
	 this.bt_save_cfg.TabIndex = 22;
	 this.bt_save_cfg.Text = "Save Config";
	 this.bt_save_cfg.UseVisualStyleBackColor = true;
	 this.bt_save_cfg.Click += new System.EventHandler(this.bt_save_cfg_Click);
	 // 
	 // tb_debug
	 // 
	 this.tb_debug.Location = new System.Drawing.Point(34, 429);
	 this.tb_debug.Name = "tb_debug";
	 this.tb_debug.Size = new System.Drawing.Size(47, 20);
	 this.tb_debug.TabIndex = 23;
	 // 
	 // label14
	 // 
	 this.label14.AutoSize = true;
	 this.label14.Location = new System.Drawing.Point(5, 116);
	 this.label14.Name = "label14";
	 this.label14.Size = new System.Drawing.Size(39, 13);
	 this.label14.TabIndex = 24;
	 this.label14.Text = "Facing";
	 // 
	 // label15
	 // 
	 this.label15.AutoSize = true;
	 this.label15.Location = new System.Drawing.Point(6, 142);
	 this.label15.Name = "label15";
	 this.label15.Size = new System.Drawing.Size(38, 13);
	 this.label15.TabIndex = 25;
	 this.label15.Text = "Speed";
	 // 
	 // lb_delta
	 // 
	 this.lb_delta.AutoSize = true;
	 this.lb_delta.Location = new System.Drawing.Point(58, 452);
	 this.lb_delta.Name = "lb_delta";
	 this.lb_delta.Size = new System.Drawing.Size(13, 13);
	 this.lb_delta.TabIndex = 26;
	 this.lb_delta.Text = "0";
	 // 
	 // lb_yaw
	 // 
	 this.lb_yaw.AutoSize = true;
	 this.lb_yaw.Location = new System.Drawing.Point(6, 452);
	 this.lb_yaw.Name = "lb_yaw";
	 this.lb_yaw.Size = new System.Drawing.Size(13, 13);
	 this.lb_yaw.TabIndex = 27;
	 this.lb_yaw.Text = "0";
	 // 
	 // button12
	 // 
	 this.button12.Location = new System.Drawing.Point(3, 61);
	 this.button12.Name = "button12";
	 this.button12.Size = new System.Drawing.Size(60, 23);
	 this.button12.TabIndex = 29;
	 this.button12.Text = "Rotation Bot";
	 this.button12.UseVisualStyleBackColor = true;
	 this.button12.Click += new System.EventHandler(this.button12_Click);
	 // 
	 // tb_loot_tries
	 // 
	 this.tb_loot_tries.Location = new System.Drawing.Point(49, 293);
	 this.tb_loot_tries.Name = "tb_loot_tries";
	 this.tb_loot_tries.Size = new System.Drawing.Size(45, 20);
	 this.tb_loot_tries.TabIndex = 30;
	 this.tb_loot_tries.Text = "200";
	 // 
	 // label22
	 // 
	 this.label22.AutoSize = true;
	 this.label22.Location = new System.Drawing.Point(13, 296);
	 this.label22.Name = "label22";
	 this.label22.Size = new System.Drawing.Size(30, 13);
	 this.label22.TabIndex = 31;
	 this.label22.Text = "Tries";
	 // 
	 // button16
	 // 
	 this.button16.Location = new System.Drawing.Point(3, 238);
	 this.button16.Name = "button16";
	 this.button16.Size = new System.Drawing.Size(93, 44);
	 this.button16.TabIndex = 64;
	 this.button16.Text = "TERMINATE";
	 this.button16.UseVisualStyleBackColor = true;
	 this.button16.Click += new System.EventHandler(this.button16_Click);
	 // 
	 // button9
	 // 
	 this.button9.Location = new System.Drawing.Point(69, 62);
	 this.button9.Name = "button9";
	 this.button9.Size = new System.Drawing.Size(40, 23);
	 this.button9.TabIndex = 0;
	 this.button9.Click += new System.EventHandler(this.button9_Click_2);
	 // 
	 // button19
	 // 
	 this.button19.Location = new System.Drawing.Point(3, 218);
	 this.button19.Name = "button19";
	 this.button19.Size = new System.Drawing.Size(78, 23);
	 this.button19.TabIndex = 65;
	 this.button19.Text = "Reset Loot";
	 this.button19.UseVisualStyleBackColor = true;
	 this.button19.Click += new System.EventHandler(this.button19_Click);
	 // 
	 // tb_rogue_eat_at
	 // 
	 this.tb_rogue_eat_at.Location = new System.Drawing.Point(76, 55);
	 this.tb_rogue_eat_at.Name = "tb_rogue_eat_at";
	 this.tb_rogue_eat_at.Size = new System.Drawing.Size(26, 20);
	 this.tb_rogue_eat_at.TabIndex = 5;
	 this.tb_rogue_eat_at.Text = "50";
	 // 
	 // label25
	 // 
	 this.label25.AutoSize = true;
	 this.label25.Location = new System.Drawing.Point(35, 57);
	 this.label25.Name = "label25";
	 this.label25.Size = new System.Drawing.Size(35, 13);
	 this.label25.TabIndex = 4;
	 this.label25.Text = "Eat at";
	 this.label25.Click += new System.EventHandler(this.label25_Click);
	 // 
	 // Form1
	 // 
	 this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
	 this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	 this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
	 this.ClientSize = new System.Drawing.Size(819, 482);
	 this.Controls.Add(this.button19);
	 this.Controls.Add(this.button9);
	 this.Controls.Add(this.button16);
	 this.Controls.Add(this.label22);
	 this.Controls.Add(this.tb_loot_tries);
	 this.Controls.Add(this.button12);
	 this.Controls.Add(this.lb_yaw);
	 this.Controls.Add(this.lb_delta);
	 this.Controls.Add(this.label15);
	 this.Controls.Add(this.label14);
	 this.Controls.Add(this.tb_debug);
	 this.Controls.Add(this.bt_save_cfg);
	 this.Controls.Add(this.cb_log);
	 this.Controls.Add(this.cb_debug);
	 this.Controls.Add(this.tb_debug4);
	 this.Controls.Add(this.bt_debug2);
	 this.Controls.Add(this.tb_debug3);
	 this.Controls.Add(this.bt_anda);
	 this.Controls.Add(this.tb_debug1);
	 this.Controls.Add(this.tb_debug2);
	 this.Controls.Add(this.bt_debug1);
	 this.Controls.Add(this.bt_getstats);
	 this.Controls.Add(this.bt_loadWP);
	 this.Controls.Add(this.tb_yaw);
	 this.Controls.Add(this.tb_spd);
	 this.Controls.Add(this.tb_y);
	 this.Controls.Add(this.tb_x);
	 this.Controls.Add(this.tab_nav);
	 this.Name = "Form1";
	 this.Text = "Discord 1.0";
	 this.Load += new System.EventHandler(this.Form1_Load);
	 this.tab_nav.ResumeLayout(false);
	 this.tabPage1.ResumeLayout(false);
	 this.tabPage1.PerformLayout();
	 ((System.ComponentModel.ISupportInitialize)(this.pb_minimap)).EndInit();
	 this.tabPage2.ResumeLayout(false);
	 this.tabPage2.PerformLayout();
	 ((System.ComponentModel.ISupportInitialize)(this.pb_map)).EndInit();
	 this.tab_buffs.ResumeLayout(false);
	 this.Paladin.ResumeLayout(false);
	 this.Paladin.PerformLayout();
	 this.tabPage4.ResumeLayout(false);
	 this.tabPage4.PerformLayout();
	 this.pan_tar.ResumeLayout(false);
	 this.pan_tar.PerformLayout();
	 this.pan_me.ResumeLayout(false);
	 this.pan_me.PerformLayout();
	 this.tabPage3.ResumeLayout(false);
	 this.tabPage3.PerformLayout();
	 this.ResumeLayout(false);
	 this.PerformLayout();

	}

		#endregion

		private System.Windows.Forms.TabControl tab_nav;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox tb_hp;
		private System.Windows.Forms.TextBox tb_mana;
		private System.Windows.Forms.TextBox tb_y;
		private System.Windows.Forms.TextBox tb_x;
		private System.Windows.Forms.TextBox tb_spd;
		private System.Windows.Forms.TextBox tb_yaw;
	private System.Windows.Forms.Button bt_getstats;
	private System.Windows.Forms.Button bt_debug1;
	private System.Windows.Forms.TextBox tb_debug1;
	private System.Windows.Forms.TextBox tb_debug2;
	private System.Windows.Forms.TextBox tb_debug3;
	private System.Windows.Forms.Button bt_debug2;
	private System.Windows.Forms.TextBox tb_debug4;
	private System.Windows.Forms.Button bt_debug3;
	private System.Windows.Forms.Button bt_onoff;
	private System.Windows.Forms.ListBox lbwp;
	private System.Windows.Forms.Button button2;
	private System.Windows.Forms.Button button3;
	private System.Windows.Forms.Button bt_anda;
	private System.Windows.Forms.TextBox lb_log;
	private System.Windows.Forms.Button button4;
	private System.Windows.Forms.CheckBox cb_round;
	private System.Windows.Forms.TextBox tb_WP_distance;
	private System.Windows.Forms.Button button5;
	private System.Windows.Forms.Button button6;
	private System.Windows.Forms.CheckBox cb_nostop;
	private System.Windows.Forms.CheckBox cb_debug;
	private System.Windows.Forms.CheckBox cb_log;
	private System.Windows.Forms.Button button7;
	private System.Windows.Forms.Button bt_saveWP;
	private System.Windows.Forms.CheckBox cb_anda;
	private System.Windows.Forms.CheckBox cb_combat;
	private System.Windows.Forms.Button bt_save_cfg;
	private System.Windows.Forms.Panel pan_tar;
	private System.Windows.Forms.Panel pan_me;
	private System.Windows.Forms.TextBox tb_mood;
	private System.Windows.Forms.Label label7;
	private System.Windows.Forms.TextBox tb_tarlevel;
	private System.Windows.Forms.Label label6;
	private System.Windows.Forms.TextBox tb_tartype;
	private System.Windows.Forms.TextBox tb_tarhp;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.TextBox tb_level;
	private System.Windows.Forms.Label label5;
	private System.Windows.Forms.TextBox tb_class;
	private System.Windows.Forms.Label label3;
	private System.Windows.Forms.Label label4;
	private System.Windows.Forms.TextBox tb_playercast;
	private System.Windows.Forms.ProgressBar pb_playercast;
	private System.Windows.Forms.TextBox tb_tarcast;
	private System.Windows.Forms.ProgressBar pb_tarcast;
	private System.Windows.Forms.TabControl tab_buffs;
	private System.Windows.Forms.TabPage Paladin;
	private System.Windows.Forms.CheckBox cb_SOR;
	private System.Windows.Forms.TabPage tabPage4;
	private System.Windows.Forms.TextBox textBox1;
	private System.Windows.Forms.CheckBox cb_melee;
	private System.Windows.Forms.Label label2;
	private System.Windows.Forms.TextBox tb_debug;
	private System.Windows.Forms.CheckBox cb_BOM;
	private System.Windows.Forms.CheckBox cb_judge;
	private System.Windows.Forms.CheckBox cb_judge_range;
	private System.Windows.Forms.CheckBox cb_autoattack;
	private System.Windows.Forms.CheckBox cb_pull;
	private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.PictureBox pb_map;
		private System.Windows.Forms.Label lb_map_zoom;
		private System.Windows.Forms.TextBox tb_map_zoom;
	private System.Windows.Forms.Label lb_delta;
	private System.Windows.Forms.Label lb_yaw;
	private System.Windows.Forms.CheckBox cb_killgray;
	private System.Windows.Forms.Label label16;
	private System.Windows.Forms.TextBox tb_preheal;
	private System.Windows.Forms.CheckBox cb_hammer_range;
	private System.Windows.Forms.CheckBox cb_use_hammer;
	private System.Windows.Forms.Label label19;
	private System.Windows.Forms.TextBox tb_interrupt_at;
	private System.Windows.Forms.Button bt_loadWP;
	private System.Windows.Forms.TextBox tb_filename;
	private System.Windows.Forms.Button bt_saveWPas;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.CheckBox cb_loot;
		private System.Windows.Forms.TextBox rtbcursor;
		private System.Windows.Forms.CheckBox cb_skinning;
		private System.Windows.Forms.CheckBox cb_dwarf;
		private System.Windows.Forms.CheckBox cb_bubble;
		private System.Windows.Forms.CheckBox cb_purify;
		private System.Windows.Forms.CheckBox cb_loh;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.TextBox tb_pullcap;
		private System.Windows.Forms.TextBox tb_stoneform_at;
		private System.Windows.Forms.TextBox tb_bow_trig;
		private System.Windows.Forms.CheckBox cb_BOW;
	private System.Windows.Forms.Button button12;
		private System.Windows.Forms.CheckBox cb_nohumanoid;
		private System.Windows.Forms.CheckBox cb_nodragonkin;
	private System.Windows.Forms.CheckBox cb_BOK;
	private System.Windows.Forms.CheckBox cb_hearth_bagfull;
	private System.Windows.Forms.CheckBox cb_use_exorcism;
		private System.Windows.Forms.CheckBox cb_loot_cloth;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TextBox tb_mapname;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button13;
		private System.Windows.Forms.CheckBox cb_atualiza_mapa;
	private System.Windows.Forms.Label label9;
	private System.Windows.Forms.Label label8;
	private System.Windows.Forms.TextBox tbavdecay;
	private System.Windows.Forms.TextBox tbdecay;
	private System.Windows.Forms.Label label18;
	private System.Windows.Forms.TextBox tb_back_limiar;
	private System.Windows.Forms.TextBox tb_loot_tries;
	private System.Windows.Forms.Label label22;
	private System.Windows.Forms.CheckBox cb_concentration_aura;
	private System.Windows.Forms.PictureBox pb_minimap;
	private System.Windows.Forms.TextBox tbraio;
	private System.Windows.Forms.TextBox tby;
	private System.Windows.Forms.TextBox tbx;
	private System.Windows.Forms.Button button10;
	private Button button14;
	private Button find_dots;
	private CheckBox cb_herbalism;
	private CheckBox cb_pacifist;
	private Button button15;
	private CheckBox cb_BOSA;
	private CheckBox cb_SOL;
	private CheckBox cb_savemana;
	private TextBox tb_mana_min;
	private TextBox tb_SOL_limiar;
	private TextBox tb_BOSA_limiar;
	private CheckBox cb_devaura;
	private CheckBox cb_retaura;
	private Label label10;
	private TextBox tb_pull_mana;
	private Label label12;
	private TextBox tb_combatheal;
	private CheckBox cb_keep_JOL;
	private Button button16;
	private Label label13;
	private TextBox tb_disable_BOW;
	private CheckBox cb_wrong_gira;
	private CheckBox cb_keep_JOTC;
	private TextBox tb_JOTC_lvl;
	private CheckBox cb_HS_timer;
	private TextBox hs_min_left;
	private Button button9;
	private CheckBox cb_assist_tank;
	private CheckBox cb_BOP;
	private Button button17;
	private TextBox tb_purify_delay;
	private CheckBox cb_apagacinza;
	private Button button18;
	private TextBox tb_timer_hours;
	private CheckBox cb_flashheal;
	private CheckBox cb_humanoid_patrol;
	private CheckBox cb_giant_patrol;
	private TextBox tb_wait_patrol;
	private CheckBox cb_elite_patrol;
	private CheckBox cb_rare_patrol;
	private Button button19;
	private CheckBox debug_potion;
	private CheckBox debug_LOH;
	private CheckBox debug_dprot;
	private CheckBox debug_BOP;
	private CheckBox debug_forbearance;
	private CheckBox debug_gcd;
	private TextBox tb_motivo_cura;
	private TextBox tb_ultima_cura;
	private CheckBox debug_HOJ;
	private Label label20;
	private TextBox tb_idle_reason;
	private Label label17;
	private Label label11;
	private CheckBox cb_logar_cura;
	private Button button20;
	private TextBox tb_mobs;
	private Label label23;
	private TextBox tb_combos;
	private TextBox tb_evis_cp;
	private Label label24;
	private CheckBox cb_scan_elite;
	private TextBox tb_rogue_eat_at;
	private Label label25;
 }
}

