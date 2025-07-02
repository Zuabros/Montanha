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
	 this.tb_debug = new System.Windows.Forms.TextBox();
	 this.label38 = new System.Windows.Forms.Label();
	 this.label37 = new System.Windows.Forms.Label();
	 this.lb_combatlog = new System.Windows.Forms.TextBox();
	 this.bt_fenceA = new System.Windows.Forms.Button();
	 this.bt_savefence = new System.Windows.Forms.Button();
	 this.bt_fenceB = new System.Windows.Forms.Button();
	 this.bt_fenceP = new System.Windows.Forms.Button();
	 this.tb_mouse_drag = new System.Windows.Forms.TextBox();
	 this.button21 = new System.Windows.Forms.Button();
	 this.label28 = new System.Windows.Forms.Label();
	 this.tb_mobs = new System.Windows.Forms.TextBox();
	 this.button18 = new System.Windows.Forms.Button();
	 this.button17 = new System.Windows.Forms.Button();
	 this.cb_assist_tank = new System.Windows.Forms.CheckBox();
	 this.button15 = new System.Windows.Forms.Button();
	 this.find_dots = new System.Windows.Forms.Button();
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
	 this.button6 = new System.Windows.Forms.Button();
	 this.tb_WP_distance = new System.Windows.Forms.TextBox();
	 this.button5 = new System.Windows.Forms.Button();
	 this.cb_round = new System.Windows.Forms.CheckBox();
	 this.button4 = new System.Windows.Forms.Button();
	 this.lb_log = new System.Windows.Forms.TextBox();
	 this.button3 = new System.Windows.Forms.Button();
	 this.button2 = new System.Windows.Forms.Button();
	 this.lbwp = new System.Windows.Forms.ListBox();
	 this.tabPage2 = new System.Windows.Forms.TabPage();
	 this.cb_nomurloc = new System.Windows.Forms.CheckBox();
	 this.tb_hearthlevel = new System.Windows.Forms.TextBox();
	 this.cb_hearth_ding = new System.Windows.Forms.CheckBox();
	 this.elemental_patrol = new System.Windows.Forms.CheckBox();
	 this.cb_scan_highlevel = new System.Windows.Forms.CheckBox();
	 this.cb_no_backpedal = new System.Windows.Forms.CheckBox();
	 this.cb_nomech = new System.Windows.Forms.CheckBox();
	 this.cb_noelite = new System.Windows.Forms.CheckBox();
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
	 this.tabPage4 = new System.Windows.Forms.TabPage();
	 this.label52 = new System.Windows.Forms.Label();
	 this.tb_damage_ss = new System.Windows.Forms.TextBox();
	 this.label51 = new System.Windows.Forms.Label();
	 this.tb_damage_hit = new System.Windows.Forms.TextBox();
	 this.label50 = new System.Windows.Forms.Label();
	 this.tb_energy_ss = new System.Windows.Forms.TextBox();
	 this.cb_randomize_rogue = new System.Windows.Forms.CheckBox();
	 this.cb_expose_armor = new System.Windows.Forms.CheckBox();
	 this.cb_SAD = new System.Windows.Forms.CheckBox();
	 this.cb_evis_auto = new System.Windows.Forms.CheckBox();
	 this.cb_range_pull = new System.Windows.Forms.CheckBox();
	 this.tb_evasion = new System.Windows.Forms.TextBox();
	 this.label26 = new System.Windows.Forms.Label();
	 this.cb_stealth_pull = new System.Windows.Forms.CheckBox();
	 this.cb_pickpocket = new System.Windows.Forms.CheckBox();
	 this.tb_rogue_eat_at = new System.Windows.Forms.TextBox();
	 this.label25 = new System.Windows.Forms.Label();
	 this.tb_evis_cp = new System.Windows.Forms.TextBox();
	 this.label24 = new System.Windows.Forms.Label();
	 this.label23 = new System.Windows.Forms.Label();
	 this.tb_combos = new System.Windows.Forms.TextBox();
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
	 this.tabPage5 = new System.Windows.Forms.TabPage();
	 this.cb_sunderspam = new System.Windows.Forms.CheckBox();
	 this.cb_random_pull_warrior = new System.Windows.Forms.CheckBox();
	 this.tb_rest_warr = new System.Windows.Forms.TextBox();
	 this.label29 = new System.Windows.Forms.Label();
	 this.tb_bloorrage_maxmobs = new System.Windows.Forms.TextBox();
	 this.cb_use_bloodrage = new System.Windows.Forms.CheckBox();
	 this.cb_use_rend = new System.Windows.Forms.CheckBox();
	 this.tb_heroic_strike_rage = new System.Windows.Forms.TextBox();
	 this.label27 = new System.Windows.Forms.Label();
	 this.tb_thunderclap_count = new System.Windows.Forms.TextBox();
	 this.cb_use_thunderclap = new System.Windows.Forms.CheckBox();
	 this.cb_war_rangepull = new System.Windows.Forms.CheckBox();
	 this.cb_bersek = new System.Windows.Forms.CheckBox();
	 this.tabPage6 = new System.Windows.Forms.TabPage();
	 this.cb_autocurse = new System.Windows.Forms.CheckBox();
	 this.cb_sendpet = new System.Windows.Forms.CheckBox();
	 this.cb_wand = new System.Windows.Forms.CheckBox();
	 this.cb_drain_soul = new System.Windows.Forms.CheckBox();
	 this.cb_COA = new System.Windows.Forms.CheckBox();
	 this.label32 = new System.Windows.Forms.Label();
	 this.tb_pull_hp_lock = new System.Windows.Forms.TextBox();
	 this.cb_lifetap_auto = new System.Windows.Forms.CheckBox();
	 this.tb_lifetap_hp = new System.Windows.Forms.TextBox();
	 this.label31 = new System.Windows.Forms.Label();
	 this.tb_lifetap_mana = new System.Windows.Forms.TextBox();
	 this.label30 = new System.Windows.Forms.Label();
	 this.cb_use_immolate = new System.Windows.Forms.CheckBox();
	 this.cb_use_corruption = new System.Windows.Forms.CheckBox();
	 this.cb_COW = new System.Windows.Forms.CheckBox();
	 this.tb_shadowbolt_mana = new System.Windows.Forms.TextBox();
	 this.cb_use_shadowbolt = new System.Windows.Forms.CheckBox();
	 this.tabPage7 = new System.Windows.Forms.TabPage();
	 this.cb_shielded_pull = new System.Windows.Forms.CheckBox();
	 this.cb_combat_pws = new System.Windows.Forms.CheckBox();
	 this.cb_shielded_smite = new System.Windows.Forms.CheckBox();
	 this.label36 = new System.Windows.Forms.Label();
	 this.tb_mana_pull_priest = new System.Windows.Forms.TextBox();
	 this.tb_smitemana = new System.Windows.Forms.TextBox();
	 this.cb_use_priest_wand = new System.Windows.Forms.CheckBox();
	 this.cb_usesmite = new System.Windows.Forms.CheckBox();
	 this.label35 = new System.Windows.Forms.Label();
	 this.tb_renewat = new System.Windows.Forms.TextBox();
	 this.label33 = new System.Windows.Forms.Label();
	 this.tb_priest_combatheal = new System.Windows.Forms.TextBox();
	 this.label34 = new System.Windows.Forms.Label();
	 this.tb_priest_pullheal = new System.Windows.Forms.TextBox();
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
	 this.Statistics = new System.Windows.Forms.TabPage();
	 this.label46 = new System.Windows.Forms.Label();
	 this.tb_kills_hunter = new System.Windows.Forms.TextBox();
	 this.label45 = new System.Windows.Forms.Label();
	 this.label44 = new System.Windows.Forms.Label();
	 this.tb_kills_mage = new System.Windows.Forms.TextBox();
	 this.tb_kills_druid = new System.Windows.Forms.TextBox();
	 this.tb_kills_rogue = new System.Windows.Forms.TextBox();
	 this.tb_kills_paladin = new System.Windows.Forms.TextBox();
	 this.tb_kills_warrior = new System.Windows.Forms.TextBox();
	 this.tb_kills_warlock = new System.Windows.Forms.TextBox();
	 this.tb_kills_priest = new System.Windows.Forms.TextBox();
	 this.label43 = new System.Windows.Forms.Label();
	 this.label42 = new System.Windows.Forms.Label();
	 this.label41 = new System.Windows.Forms.Label();
	 this.label40 = new System.Windows.Forms.Label();
	 this.label39 = new System.Windows.Forms.Label();
	 this.label49 = new System.Windows.Forms.Label();
	 this.label48 = new System.Windows.Forms.Label();
	 this.label47 = new System.Windows.Forms.Label();
	 this.label22 = new System.Windows.Forms.Label();
	 this.tb_minus1 = new System.Windows.Forms.TextBox();
	 this.tb_regular = new System.Windows.Forms.TextBox();
	 this.tb_plus1 = new System.Windows.Forms.TextBox();
	 this.tb_timer_hours = new System.Windows.Forms.TextBox();
	 this.cb_HS_timer = new System.Windows.Forms.CheckBox();
	 this.hs_min_left = new System.Windows.Forms.TextBox();
	 this.bt_onoff = new System.Windows.Forms.Button();
	 this.bt_loadWP = new System.Windows.Forms.Button();
	 this.bt_anda = new System.Windows.Forms.Button();
	 this.tb_y = new System.Windows.Forms.TextBox();
	 this.tb_x = new System.Windows.Forms.TextBox();
	 this.tb_spd = new System.Windows.Forms.TextBox();
	 this.tb_yaw = new System.Windows.Forms.TextBox();
	 this.bt_getstats = new System.Windows.Forms.Button();
	 this.tb_debug1 = new System.Windows.Forms.TextBox();
	 this.tb_debug2 = new System.Windows.Forms.TextBox();
	 this.bt_debug2 = new System.Windows.Forms.Button();
	 this.bt_save_cfg = new System.Windows.Forms.Button();
	 this.label14 = new System.Windows.Forms.Label();
	 this.label15 = new System.Windows.Forms.Label();
	 this.lb_delta = new System.Windows.Forms.Label();
	 this.button12 = new System.Windows.Forms.Button();
	 this.button16 = new System.Windows.Forms.Button();
	 this.button19 = new System.Windows.Forms.Button();
	 this.cb_humanlike = new System.Windows.Forms.CheckBox();
	 this.cb_cloudemove = new System.Windows.Forms.CheckBox();
	 this.tb_prob_plus1 = new System.Windows.Forms.TextBox();
	 this.tb_prob_zero = new System.Windows.Forms.TextBox();
	 this.tb_prob_minus1 = new System.Windows.Forms.TextBox();
	 this.tabPage8 = new System.Windows.Forms.TabPage();
	 this.tab_nav.SuspendLayout();
	 this.tabPage1.SuspendLayout();
	 ((System.ComponentModel.ISupportInitialize)(this.pb_minimap)).BeginInit();
	 this.tabPage2.SuspendLayout();
	 ((System.ComponentModel.ISupportInitialize)(this.pb_map)).BeginInit();
	 this.tab_buffs.SuspendLayout();
	 this.tabPage4.SuspendLayout();
	 this.Paladin.SuspendLayout();
	 this.tabPage5.SuspendLayout();
	 this.tabPage6.SuspendLayout();
	 this.tabPage7.SuspendLayout();
	 this.pan_tar.SuspendLayout();
	 this.pan_me.SuspendLayout();
	 this.tabPage3.SuspendLayout();
	 this.Statistics.SuspendLayout();
	 this.SuspendLayout();
	 // 
	 // tab_nav
	 // 
	 this.tab_nav.Controls.Add(this.tabPage1);
	 this.tab_nav.Controls.Add(this.tabPage2);
	 this.tab_nav.Controls.Add(this.tabPage3);
	 this.tab_nav.Controls.Add(this.Statistics);
	 this.tab_nav.Location = new System.Drawing.Point(115, 4);
	 this.tab_nav.Name = "tab_nav";
	 this.tab_nav.SelectedIndex = 0;
	 this.tab_nav.Size = new System.Drawing.Size(705, 528);
	 this.tab_nav.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
	 this.tab_nav.TabIndex = 0;
	 // 
	 // tabPage1
	 // 
	 this.tabPage1.BackColor = System.Drawing.Color.DimGray;
	 this.tabPage1.Controls.Add(this.tb_debug);
	 this.tabPage1.Controls.Add(this.label38);
	 this.tabPage1.Controls.Add(this.label37);
	 this.tabPage1.Controls.Add(this.lb_combatlog);
	 this.tabPage1.Controls.Add(this.bt_fenceA);
	 this.tabPage1.Controls.Add(this.bt_savefence);
	 this.tabPage1.Controls.Add(this.bt_fenceB);
	 this.tabPage1.Controls.Add(this.bt_fenceP);
	 this.tabPage1.Controls.Add(this.tb_mouse_drag);
	 this.tabPage1.Controls.Add(this.button21);
	 this.tabPage1.Controls.Add(this.label28);
	 this.tabPage1.Controls.Add(this.tb_mobs);
	 this.tabPage1.Controls.Add(this.button18);
	 this.tabPage1.Controls.Add(this.button17);
	 this.tabPage1.Controls.Add(this.cb_assist_tank);
	 this.tabPage1.Controls.Add(this.button15);
	 this.tabPage1.Controls.Add(this.find_dots);
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
	 this.tabPage1.Controls.Add(this.button6);
	 this.tabPage1.Controls.Add(this.tb_WP_distance);
	 this.tabPage1.Controls.Add(this.button5);
	 this.tabPage1.Controls.Add(this.cb_round);
	 this.tabPage1.Controls.Add(this.button4);
	 this.tabPage1.Controls.Add(this.lb_log);
	 this.tabPage1.Controls.Add(this.button3);
	 this.tabPage1.Controls.Add(this.button2);
	 this.tabPage1.Controls.Add(this.lbwp);
	 this.tabPage1.Location = new System.Drawing.Point(4, 22);
	 this.tabPage1.Name = "tabPage1";
	 this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
	 this.tabPage1.Size = new System.Drawing.Size(697, 502);
	 this.tabPage1.TabIndex = 0;
	 this.tabPage1.Text = "Nav";
	 // 
	 // tb_debug
	 // 
	 this.tb_debug.Location = new System.Drawing.Point(619, 138);
	 this.tb_debug.Name = "tb_debug";
	 this.tb_debug.Size = new System.Drawing.Size(69, 20);
	 this.tb_debug.TabIndex = 67;
	 this.tb_debug.Text = "1";
	 // 
	 // label38
	 // 
	 this.label38.AutoSize = true;
	 this.label38.Location = new System.Drawing.Point(225, 293);
	 this.label38.Name = "label38";
	 this.label38.Size = new System.Drawing.Size(65, 13);
	 this.label38.TabIndex = 66;
	 this.label38.Text = "General Log";
	 // 
	 // label37
	 // 
	 this.label37.AutoSize = true;
	 this.label37.Location = new System.Drawing.Point(7, 293);
	 this.label37.Name = "label37";
	 this.label37.Size = new System.Drawing.Size(64, 13);
	 this.label37.TabIndex = 65;
	 this.label37.Text = "Combat Log";
	 // 
	 // lb_combatlog
	 // 
	 this.lb_combatlog.BackColor = System.Drawing.SystemColors.ScrollBar;
	 this.lb_combatlog.Location = new System.Drawing.Point(3, 309);
	 this.lb_combatlog.Multiline = true;
	 this.lb_combatlog.Name = "lb_combatlog";
	 this.lb_combatlog.Size = new System.Drawing.Size(252, 187);
	 this.lb_combatlog.TabIndex = 64;
	 // 
	 // bt_fenceA
	 // 
	 this.bt_fenceA.Location = new System.Drawing.Point(113, 90);
	 this.bt_fenceA.Name = "bt_fenceA";
	 this.bt_fenceA.Size = new System.Drawing.Size(83, 23);
	 this.bt_fenceA.TabIndex = 63;
	 this.bt_fenceA.Text = "Fence A";
	 this.bt_fenceA.UseVisualStyleBackColor = true;
	 this.bt_fenceA.Click += new System.EventHandler(this.bt_fenceA_Click);
	 // 
	 // bt_savefence
	 // 
	 this.bt_savefence.Location = new System.Drawing.Point(202, 115);
	 this.bt_savefence.Name = "bt_savefence";
	 this.bt_savefence.Size = new System.Drawing.Size(75, 23);
	 this.bt_savefence.TabIndex = 62;
	 this.bt_savefence.Text = "Add Fence";
	 this.bt_savefence.UseVisualStyleBackColor = true;
	 this.bt_savefence.Click += new System.EventHandler(this.bt_savefence_Click);
	 // 
	 // bt_fenceB
	 // 
	 this.bt_fenceB.Location = new System.Drawing.Point(201, 90);
	 this.bt_fenceB.Name = "bt_fenceB";
	 this.bt_fenceB.Size = new System.Drawing.Size(75, 23);
	 this.bt_fenceB.TabIndex = 61;
	 this.bt_fenceB.Text = "Fence B";
	 this.bt_fenceB.UseVisualStyleBackColor = true;
	 this.bt_fenceB.Click += new System.EventHandler(this.bt_fenceB_Click);
	 // 
	 // bt_fenceP
	 // 
	 this.bt_fenceP.Location = new System.Drawing.Point(113, 115);
	 this.bt_fenceP.Name = "bt_fenceP";
	 this.bt_fenceP.Size = new System.Drawing.Size(83, 23);
	 this.bt_fenceP.TabIndex = 60;
	 this.bt_fenceP.Text = "Fence P";
	 this.bt_fenceP.UseVisualStyleBackColor = true;
	 this.bt_fenceP.Click += new System.EventHandler(this.bt_fenceP_Click);
	 // 
	 // tb_mouse_drag
	 // 
	 this.tb_mouse_drag.Location = new System.Drawing.Point(537, 77);
	 this.tb_mouse_drag.Name = "tb_mouse_drag";
	 this.tb_mouse_drag.Size = new System.Drawing.Size(48, 20);
	 this.tb_mouse_drag.TabIndex = 59;
	 this.tb_mouse_drag.Text = "30";
	 // 
	 // button21
	 // 
	 this.button21.Location = new System.Drawing.Point(514, 169);
	 this.button21.Name = "button21";
	 this.button21.Size = new System.Drawing.Size(90, 23);
	 this.button21.TabIndex = 58;
	 this.button21.Text = "Giralvo";
	 this.button21.UseVisualStyleBackColor = true;
	 this.button21.Click += new System.EventHandler(this.button21_Click);
	 // 
	 // label28
	 // 
	 this.label28.AutoSize = true;
	 this.label28.Location = new System.Drawing.Point(501, 265);
	 this.label28.Name = "label28";
	 this.label28.Size = new System.Drawing.Size(56, 13);
	 this.label28.TabIndex = 57;
	 this.label28.Text = "SPD: 21,6";
	 // 
	 // tb_mobs
	 // 
	 this.tb_mobs.Location = new System.Drawing.Point(537, 103);
	 this.tb_mobs.Name = "tb_mobs";
	 this.tb_mobs.Size = new System.Drawing.Size(100, 20);
	 this.tb_mobs.TabIndex = 56;
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
	 this.cb_assist_tank.Location = new System.Drawing.Point(514, 140);
	 this.cb_assist_tank.Name = "cb_assist_tank";
	 this.cb_assist_tank.Size = new System.Drawing.Size(77, 17);
	 this.cb_assist_tank.TabIndex = 52;
	 this.cb_assist_tank.Text = "Asisst tank";
	 this.cb_assist_tank.UseVisualStyleBackColor = true;
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
	 this.tbraio.Location = new System.Drawing.Point(419, 22);
	 this.tbraio.Name = "tbraio";
	 this.tbraio.Size = new System.Drawing.Size(52, 20);
	 this.tbraio.TabIndex = 43;
	 this.tbraio.Text = "163";
	 // 
	 // tby
	 // 
	 this.tby.Location = new System.Drawing.Point(366, 23);
	 this.tby.Name = "tby";
	 this.tby.Size = new System.Drawing.Size(47, 20);
	 this.tby.TabIndex = 42;
	 this.tby.Text = "150";
	 // 
	 // tbx
	 // 
	 this.tbx.Location = new System.Drawing.Point(308, 23);
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
	 this.button11.Text = "Remove";
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
	 this.tb_filename.Location = new System.Drawing.Point(202, 4);
	 this.tb_filename.Name = "tb_filename";
	 this.tb_filename.Size = new System.Drawing.Size(258, 20);
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
	 // button6
	 // 
	 this.button6.Location = new System.Drawing.Point(113, 144);
	 this.button6.Name = "button6";
	 this.button6.Size = new System.Drawing.Size(75, 23);
	 this.button6.TabIndex = 11;
	 this.button6.Text = "Clear";
	 this.button6.UseVisualStyleBackColor = true;
	 this.button6.Click += new System.EventHandler(this.button6_Click);
	 // 
	 // tb_WP_distance
	 // 
	 this.tb_WP_distance.Location = new System.Drawing.Point(202, 189);
	 this.tb_WP_distance.Name = "tb_WP_distance";
	 this.tb_WP_distance.Size = new System.Drawing.Size(31, 20);
	 this.tb_WP_distance.TabIndex = 10;
	 this.tb_WP_distance.Text = "150";
	 // 
	 // button5
	 // 
	 this.button5.Location = new System.Drawing.Point(120, 183);
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
	 this.cb_round.Location = new System.Drawing.Point(113, 212);
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
	 this.lb_log.Location = new System.Drawing.Point(261, 309);
	 this.lb_log.Multiline = true;
	 this.lb_log.Name = "lb_log";
	 this.lb_log.Size = new System.Drawing.Size(412, 187);
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
	 this.lbwp.Location = new System.Drawing.Point(8, 6);
	 this.lbwp.Name = "lbwp";
	 this.lbwp.Size = new System.Drawing.Size(98, 277);
	 this.lbwp.TabIndex = 1;
	 // 
	 // tabPage2
	 // 
	 this.tabPage2.BackColor = System.Drawing.Color.Gray;
	 this.tabPage2.Controls.Add(this.cb_nomurloc);
	 this.tabPage2.Controls.Add(this.tb_hearthlevel);
	 this.tabPage2.Controls.Add(this.cb_hearth_ding);
	 this.tabPage2.Controls.Add(this.elemental_patrol);
	 this.tabPage2.Controls.Add(this.cb_scan_highlevel);
	 this.tabPage2.Controls.Add(this.cb_no_backpedal);
	 this.tabPage2.Controls.Add(this.cb_nomech);
	 this.tabPage2.Controls.Add(this.cb_noelite);
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
	 this.tabPage2.Size = new System.Drawing.Size(697, 502);
	 this.tabPage2.TabIndex = 1;
	 this.tabPage2.Text = "Combat";
	 // 
	 // cb_nomurloc
	 // 
	 this.cb_nomurloc.AutoSize = true;
	 this.cb_nomurloc.Checked = true;
	 this.cb_nomurloc.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_nomurloc.Location = new System.Drawing.Point(532, 42);
	 this.cb_nomurloc.Name = "cb_nomurloc";
	 this.cb_nomurloc.Size = new System.Drawing.Size(75, 17);
	 this.cb_nomurloc.TabIndex = 79;
	 this.cb_nomurloc.Text = "No Murloc";
	 this.cb_nomurloc.UseVisualStyleBackColor = true;
	 this.cb_nomurloc.CheckedChanged += new System.EventHandler(this.cb_nomurloc_CheckedChanged);
	 // 
	 // tb_hearthlevel
	 // 
	 this.tb_hearthlevel.Location = new System.Drawing.Point(90, 463);
	 this.tb_hearthlevel.Name = "tb_hearthlevel";
	 this.tb_hearthlevel.Size = new System.Drawing.Size(31, 20);
	 this.tb_hearthlevel.TabIndex = 78;
	 this.tb_hearthlevel.Text = "8";
	 // 
	 // cb_hearth_ding
	 // 
	 this.cb_hearth_ding.AutoSize = true;
	 this.cb_hearth_ding.Location = new System.Drawing.Point(9, 465);
	 this.cb_hearth_ding.Name = "cb_hearth_ding";
	 this.cb_hearth_ding.Size = new System.Drawing.Size(83, 17);
	 this.cb_hearth_ding.TabIndex = 77;
	 this.cb_hearth_ding.Text = "Hearth at lvl";
	 this.cb_hearth_ding.UseVisualStyleBackColor = true;
	 // 
	 // elemental_patrol
	 // 
	 this.elemental_patrol.AutoSize = true;
	 this.elemental_patrol.Checked = true;
	 this.elemental_patrol.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.elemental_patrol.Location = new System.Drawing.Point(6, 368);
	 this.elemental_patrol.Name = "elemental_patrol";
	 this.elemental_patrol.Size = new System.Drawing.Size(72, 17);
	 this.elemental_patrol.TabIndex = 76;
	 this.elemental_patrol.Text = "Elemental";
	 this.elemental_patrol.UseVisualStyleBackColor = true;
	 // 
	 // cb_scan_highlevel
	 // 
	 this.cb_scan_highlevel.AutoSize = true;
	 this.cb_scan_highlevel.Checked = true;
	 this.cb_scan_highlevel.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_scan_highlevel.Location = new System.Drawing.Point(6, 344);
	 this.cb_scan_highlevel.Name = "cb_scan_highlevel";
	 this.cb_scan_highlevel.Size = new System.Drawing.Size(77, 17);
	 this.cb_scan_highlevel.TabIndex = 75;
	 this.cb_scan_highlevel.Text = "High Level";
	 this.cb_scan_highlevel.UseVisualStyleBackColor = true;
	 // 
	 // cb_no_backpedal
	 // 
	 this.cb_no_backpedal.AutoSize = true;
	 this.cb_no_backpedal.Location = new System.Drawing.Point(9, 441);
	 this.cb_no_backpedal.Name = "cb_no_backpedal";
	 this.cb_no_backpedal.Size = new System.Drawing.Size(156, 17);
	 this.cb_no_backpedal.TabIndex = 74;
	 this.cb_no_backpedal.Text = "Caster Farm (no backpedal)";
	 this.cb_no_backpedal.UseVisualStyleBackColor = true;
	 // 
	 // cb_nomech
	 // 
	 this.cb_nomech.AutoSize = true;
	 this.cb_nomech.Checked = true;
	 this.cb_nomech.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_nomech.Location = new System.Drawing.Point(532, 29);
	 this.cb_nomech.Name = "cb_nomech";
	 this.cb_nomech.Size = new System.Drawing.Size(98, 17);
	 this.cb_nomech.TabIndex = 73;
	 this.cb_nomech.Text = "No Mechanical";
	 this.cb_nomech.UseVisualStyleBackColor = true;
	 // 
	 // cb_noelite
	 // 
	 this.cb_noelite.AutoSize = true;
	 this.cb_noelite.Checked = true;
	 this.cb_noelite.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_noelite.Location = new System.Drawing.Point(432, 41);
	 this.cb_noelite.Name = "cb_noelite";
	 this.cb_noelite.Size = new System.Drawing.Size(63, 17);
	 this.cb_noelite.TabIndex = 72;
	 this.cb_noelite.Text = "No Elite";
	 this.cb_noelite.UseVisualStyleBackColor = true;
	 // 
	 // cb_scan_elite
	 // 
	 this.cb_scan_elite.AutoSize = true;
	 this.cb_scan_elite.Checked = true;
	 this.cb_scan_elite.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_scan_elite.Location = new System.Drawing.Point(3, 233);
	 this.cb_scan_elite.Name = "cb_scan_elite";
	 this.cb_scan_elite.Size = new System.Drawing.Size(84, 17);
	 this.cb_scan_elite.TabIndex = 71;
	 this.cb_scan_elite.Text = "Scan Active";
	 this.cb_scan_elite.UseVisualStyleBackColor = true;
	 this.cb_scan_elite.CheckedChanged += new System.EventHandler(this.cb_scan_elite_CheckedChanged);
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
	 this.tb_wait_patrol.Location = new System.Drawing.Point(77, 250);
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
	 this.cb_wrong_gira.Enabled = false;
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
	 this.lb_map_zoom.Location = new System.Drawing.Point(3, 253);
	 this.lb_map_zoom.Name = "lb_map_zoom";
	 this.lb_map_zoom.Size = new System.Drawing.Size(75, 13);
	 this.lb_map_zoom.TabIndex = 28;
	 this.lb_map_zoom.Text = "Wait seconds:";
	 // 
	 // tb_map_zoom
	 // 
	 this.tb_map_zoom.Location = new System.Drawing.Point(256, 433);
	 this.tb_map_zoom.Name = "tb_map_zoom";
	 this.tb_map_zoom.Size = new System.Drawing.Size(43, 20);
	 this.tb_map_zoom.TabIndex = 27;
	 this.tb_map_zoom.Text = "100";
	 // 
	 // tab_buffs
	 // 
	 this.tab_buffs.Controls.Add(this.tabPage4);
	 this.tab_buffs.Controls.Add(this.Paladin);
	 this.tab_buffs.Controls.Add(this.tabPage5);
	 this.tab_buffs.Controls.Add(this.tabPage6);
	 this.tab_buffs.Controls.Add(this.tabPage7);
	 this.tab_buffs.Controls.Add(this.tabPage8);
	 this.tab_buffs.Location = new System.Drawing.Point(315, 80);
	 this.tab_buffs.Name = "tab_buffs";
	 this.tab_buffs.SelectedIndex = 0;
	 this.tab_buffs.Size = new System.Drawing.Size(359, 373);
	 this.tab_buffs.TabIndex = 25;
	 // 
	 // tabPage4
	 // 
	 this.tabPage4.BackColor = System.Drawing.Color.Gray;
	 this.tabPage4.Controls.Add(this.label52);
	 this.tabPage4.Controls.Add(this.tb_damage_ss);
	 this.tabPage4.Controls.Add(this.label51);
	 this.tabPage4.Controls.Add(this.tb_damage_hit);
	 this.tabPage4.Controls.Add(this.label50);
	 this.tabPage4.Controls.Add(this.tb_energy_ss);
	 this.tabPage4.Controls.Add(this.cb_randomize_rogue);
	 this.tabPage4.Controls.Add(this.cb_expose_armor);
	 this.tabPage4.Controls.Add(this.cb_SAD);
	 this.tabPage4.Controls.Add(this.cb_evis_auto);
	 this.tabPage4.Controls.Add(this.cb_range_pull);
	 this.tabPage4.Controls.Add(this.tb_evasion);
	 this.tabPage4.Controls.Add(this.label26);
	 this.tabPage4.Controls.Add(this.cb_stealth_pull);
	 this.tabPage4.Controls.Add(this.cb_pickpocket);
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
	 // label52
	 // 
	 this.label52.AutoSize = true;
	 this.label52.Location = new System.Drawing.Point(3, 281);
	 this.label52.Name = "label52";
	 this.label52.Size = new System.Drawing.Size(67, 13);
	 this.label52.TabIndex = 20;
	 this.label52.Text = "Damage SS:";
	 // 
	 // tb_damage_ss
	 // 
	 this.tb_damage_ss.Location = new System.Drawing.Point(76, 276);
	 this.tb_damage_ss.Name = "tb_damage_ss";
	 this.tb_damage_ss.Size = new System.Drawing.Size(40, 20);
	 this.tb_damage_ss.TabIndex = 19;
	 this.tb_damage_ss.Text = "14";
	 // 
	 // label51
	 // 
	 this.label51.AutoSize = true;
	 this.label51.Location = new System.Drawing.Point(3, 256);
	 this.label51.Name = "label51";
	 this.label51.Size = new System.Drawing.Size(64, 13);
	 this.label51.TabIndex = 18;
	 this.label51.Text = "Damage hit:";
	 // 
	 // tb_damage_hit
	 // 
	 this.tb_damage_hit.Location = new System.Drawing.Point(76, 250);
	 this.tb_damage_hit.Name = "tb_damage_hit";
	 this.tb_damage_hit.Size = new System.Drawing.Size(40, 20);
	 this.tb_damage_hit.TabIndex = 17;
	 this.tb_damage_hit.Text = "10";
	 // 
	 // label50
	 // 
	 this.label50.AutoSize = true;
	 this.label50.Location = new System.Drawing.Point(1, 230);
	 this.label50.Name = "label50";
	 this.label50.Size = new System.Drawing.Size(63, 13);
	 this.label50.TabIndex = 16;
	 this.label50.Text = "Energy SS: ";
	 // 
	 // tb_energy_ss
	 // 
	 this.tb_energy_ss.Location = new System.Drawing.Point(76, 227);
	 this.tb_energy_ss.Name = "tb_energy_ss";
	 this.tb_energy_ss.Size = new System.Drawing.Size(40, 20);
	 this.tb_energy_ss.TabIndex = 15;
	 this.tb_energy_ss.Text = "45";
	 // 
	 // cb_randomize_rogue
	 // 
	 this.cb_randomize_rogue.AutoSize = true;
	 this.cb_randomize_rogue.Checked = true;
	 this.cb_randomize_rogue.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_randomize_rogue.Location = new System.Drawing.Point(223, 74);
	 this.cb_randomize_rogue.Name = "cb_randomize_rogue";
	 this.cb_randomize_rogue.Size = new System.Drawing.Size(108, 17);
	 this.cb_randomize_rogue.TabIndex = 14;
	 this.cb_randomize_rogue.Text = "Random pull type";
	 this.cb_randomize_rogue.UseVisualStyleBackColor = true;
	 // 
	 // cb_expose_armor
	 // 
	 this.cb_expose_armor.AutoSize = true;
	 this.cb_expose_armor.Checked = true;
	 this.cb_expose_armor.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_expose_armor.Location = new System.Drawing.Point(10, 118);
	 this.cb_expose_armor.Name = "cb_expose_armor";
	 this.cb_expose_armor.Size = new System.Drawing.Size(90, 17);
	 this.cb_expose_armor.TabIndex = 13;
	 this.cb_expose_armor.Text = "Expose armor";
	 this.cb_expose_armor.UseVisualStyleBackColor = true;
	 // 
	 // cb_SAD
	 // 
	 this.cb_SAD.AutoSize = true;
	 this.cb_SAD.Checked = true;
	 this.cb_SAD.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_SAD.Location = new System.Drawing.Point(10, 100);
	 this.cb_SAD.Name = "cb_SAD";
	 this.cb_SAD.Size = new System.Drawing.Size(95, 17);
	 this.cb_SAD.TabIndex = 12;
	 this.cb_SAD.Text = "Slice and Dice";
	 this.cb_SAD.UseVisualStyleBackColor = true;
	 // 
	 // cb_evis_auto
	 // 
	 this.cb_evis_auto.AutoSize = true;
	 this.cb_evis_auto.Checked = true;
	 this.cb_evis_auto.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_evis_auto.Location = new System.Drawing.Point(109, 37);
	 this.cb_evis_auto.Name = "cb_evis_auto";
	 this.cb_evis_auto.Size = new System.Drawing.Size(48, 17);
	 this.cb_evis_auto.TabIndex = 11;
	 this.cb_evis_auto.Text = "Auto";
	 this.cb_evis_auto.UseVisualStyleBackColor = true;
	 // 
	 // cb_range_pull
	 // 
	 this.cb_range_pull.AutoSize = true;
	 this.cb_range_pull.Location = new System.Drawing.Point(223, 53);
	 this.cb_range_pull.Name = "cb_range_pull";
	 this.cb_range_pull.Size = new System.Drawing.Size(84, 17);
	 this.cb_range_pull.TabIndex = 10;
	 this.cb_range_pull.Text = "Ranged Pull";
	 this.cb_range_pull.UseVisualStyleBackColor = true;
	 // 
	 // tb_evasion
	 // 
	 this.tb_evasion.Location = new System.Drawing.Point(76, 74);
	 this.tb_evasion.Name = "tb_evasion";
	 this.tb_evasion.Size = new System.Drawing.Size(26, 20);
	 this.tb_evasion.TabIndex = 9;
	 this.tb_evasion.Text = "60";
	 // 
	 // label26
	 // 
	 this.label26.AutoSize = true;
	 this.label26.Location = new System.Drawing.Point(19, 77);
	 this.label26.Name = "label26";
	 this.label26.Size = new System.Drawing.Size(57, 13);
	 this.label26.TabIndex = 8;
	 this.label26.Text = "Evasion at";
	 // 
	 // cb_stealth_pull
	 // 
	 this.cb_stealth_pull.AutoSize = true;
	 this.cb_stealth_pull.Checked = true;
	 this.cb_stealth_pull.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_stealth_pull.Location = new System.Drawing.Point(223, 29);
	 this.cb_stealth_pull.Name = "cb_stealth_pull";
	 this.cb_stealth_pull.Size = new System.Drawing.Size(79, 17);
	 this.cb_stealth_pull.TabIndex = 7;
	 this.cb_stealth_pull.Text = "Stealth Pull";
	 this.cb_stealth_pull.UseVisualStyleBackColor = true;
	 // 
	 // cb_pickpocket
	 // 
	 this.cb_pickpocket.AutoSize = true;
	 this.cb_pickpocket.Checked = true;
	 this.cb_pickpocket.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_pickpocket.Location = new System.Drawing.Point(223, 11);
	 this.cb_pickpocket.Name = "cb_pickpocket";
	 this.cb_pickpocket.Size = new System.Drawing.Size(80, 17);
	 this.cb_pickpocket.TabIndex = 6;
	 this.cb_pickpocket.Text = "Pickpocket";
	 this.cb_pickpocket.UseVisualStyleBackColor = true;
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
	 // tabPage5
	 // 
	 this.tabPage5.BackColor = System.Drawing.Color.Gray;
	 this.tabPage5.Controls.Add(this.cb_sunderspam);
	 this.tabPage5.Controls.Add(this.cb_random_pull_warrior);
	 this.tabPage5.Controls.Add(this.tb_rest_warr);
	 this.tabPage5.Controls.Add(this.label29);
	 this.tabPage5.Controls.Add(this.tb_bloorrage_maxmobs);
	 this.tabPage5.Controls.Add(this.cb_use_bloodrage);
	 this.tabPage5.Controls.Add(this.cb_use_rend);
	 this.tabPage5.Controls.Add(this.tb_heroic_strike_rage);
	 this.tabPage5.Controls.Add(this.label27);
	 this.tabPage5.Controls.Add(this.tb_thunderclap_count);
	 this.tabPage5.Controls.Add(this.cb_use_thunderclap);
	 this.tabPage5.Controls.Add(this.cb_war_rangepull);
	 this.tabPage5.Controls.Add(this.cb_bersek);
	 this.tabPage5.Location = new System.Drawing.Point(4, 22);
	 this.tabPage5.Name = "tabPage5";
	 this.tabPage5.Size = new System.Drawing.Size(351, 347);
	 this.tabPage5.TabIndex = 2;
	 this.tabPage5.Text = "Warrior";
	 // 
	 // cb_sunderspam
	 // 
	 this.cb_sunderspam.AutoSize = true;
	 this.cb_sunderspam.Checked = true;
	 this.cb_sunderspam.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_sunderspam.Location = new System.Drawing.Point(14, 167);
	 this.cb_sunderspam.Name = "cb_sunderspam";
	 this.cb_sunderspam.Size = new System.Drawing.Size(120, 17);
	 this.cb_sunderspam.TabIndex = 21;
	 this.cb_sunderspam.Text = "Spam Sunder Armor";
	 this.cb_sunderspam.UseVisualStyleBackColor = true;
	 // 
	 // cb_random_pull_warrior
	 // 
	 this.cb_random_pull_warrior.AutoSize = true;
	 this.cb_random_pull_warrior.Checked = true;
	 this.cb_random_pull_warrior.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_random_pull_warrior.Location = new System.Drawing.Point(158, 34);
	 this.cb_random_pull_warrior.Name = "cb_random_pull_warrior";
	 this.cb_random_pull_warrior.Size = new System.Drawing.Size(98, 17);
	 this.cb_random_pull_warrior.TabIndex = 11;
	 this.cb_random_pull_warrior.Text = "Randomize pull";
	 this.cb_random_pull_warrior.UseVisualStyleBackColor = true;
	 // 
	 // tb_rest_warr
	 // 
	 this.tb_rest_warr.Location = new System.Drawing.Point(99, 135);
	 this.tb_rest_warr.Name = "tb_rest_warr";
	 this.tb_rest_warr.Size = new System.Drawing.Size(20, 20);
	 this.tb_rest_warr.TabIndex = 10;
	 this.tb_rest_warr.Text = "80";
	 // 
	 // label29
	 // 
	 this.label29.AutoSize = true;
	 this.label29.Location = new System.Drawing.Point(49, 138);
	 this.label29.Name = "label29";
	 this.label29.Size = new System.Drawing.Size(44, 13);
	 this.label29.TabIndex = 9;
	 this.label29.Text = "Rest at:";
	 // 
	 // tb_bloorrage_maxmobs
	 // 
	 this.tb_bloorrage_maxmobs.Location = new System.Drawing.Point(136, 114);
	 this.tb_bloorrage_maxmobs.Name = "tb_bloorrage_maxmobs";
	 this.tb_bloorrage_maxmobs.Size = new System.Drawing.Size(15, 20);
	 this.tb_bloorrage_maxmobs.TabIndex = 8;
	 this.tb_bloorrage_maxmobs.Text = "1";
	 // 
	 // cb_use_bloodrage
	 // 
	 this.cb_use_bloodrage.AutoSize = true;
	 this.cb_use_bloodrage.Checked = true;
	 this.cb_use_bloodrage.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_use_bloodrage.Location = new System.Drawing.Point(13, 116);
	 this.cb_use_bloodrage.Name = "cb_use_bloodrage";
	 this.cb_use_bloodrage.Size = new System.Drawing.Size(121, 17);
	 this.cb_use_bloodrage.TabIndex = 7;
	 this.cb_use_bloodrage.Text = "Bloorrage max mobs";
	 this.cb_use_bloodrage.UseVisualStyleBackColor = true;
	 // 
	 // cb_use_rend
	 // 
	 this.cb_use_rend.AutoSize = true;
	 this.cb_use_rend.Checked = true;
	 this.cb_use_rend.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_use_rend.Location = new System.Drawing.Point(13, 92);
	 this.cb_use_rend.Name = "cb_use_rend";
	 this.cb_use_rend.Size = new System.Drawing.Size(87, 17);
	 this.cb_use_rend.TabIndex = 6;
	 this.cb_use_rend.Text = "Rend (bleed)";
	 this.cb_use_rend.UseVisualStyleBackColor = true;
	 // 
	 // tb_heroic_strike_rage
	 // 
	 this.tb_heroic_strike_rage.Location = new System.Drawing.Point(99, 72);
	 this.tb_heroic_strike_rage.Name = "tb_heroic_strike_rage";
	 this.tb_heroic_strike_rage.Size = new System.Drawing.Size(20, 20);
	 this.tb_heroic_strike_rage.TabIndex = 5;
	 this.tb_heroic_strike_rage.Text = "30";
	 // 
	 // label27
	 // 
	 this.label27.AutoSize = true;
	 this.label27.Location = new System.Drawing.Point(13, 75);
	 this.label27.Name = "label27";
	 this.label27.Size = new System.Drawing.Size(80, 13);
	 this.label27.TabIndex = 4;
	 this.label27.Text = "Heroic Strike at";
	 // 
	 // tb_thunderclap_count
	 // 
	 this.tb_thunderclap_count.Location = new System.Drawing.Point(104, 52);
	 this.tb_thunderclap_count.Name = "tb_thunderclap_count";
	 this.tb_thunderclap_count.Size = new System.Drawing.Size(15, 20);
	 this.tb_thunderclap_count.TabIndex = 3;
	 this.tb_thunderclap_count.Text = "3";
	 // 
	 // cb_use_thunderclap
	 // 
	 this.cb_use_thunderclap.AutoSize = true;
	 this.cb_use_thunderclap.Checked = true;
	 this.cb_use_thunderclap.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_use_thunderclap.Location = new System.Drawing.Point(13, 55);
	 this.cb_use_thunderclap.Name = "cb_use_thunderclap";
	 this.cb_use_thunderclap.Size = new System.Drawing.Size(95, 17);
	 this.cb_use_thunderclap.TabIndex = 2;
	 this.cb_use_thunderclap.Text = "Thundeclap at";
	 this.cb_use_thunderclap.UseVisualStyleBackColor = true;
	 // 
	 // cb_war_rangepull
	 // 
	 this.cb_war_rangepull.AutoSize = true;
	 this.cb_war_rangepull.Location = new System.Drawing.Point(14, 35);
	 this.cb_war_rangepull.Name = "cb_war_rangepull";
	 this.cb_war_rangepull.Size = new System.Drawing.Size(138, 17);
	 this.cb_war_rangepull.TabIndex = 1;
	 this.cb_war_rangepull.Text = "Ranged pull (Crossbow)";
	 this.cb_war_rangepull.UseVisualStyleBackColor = true;
	 // 
	 // cb_bersek
	 // 
	 this.cb_bersek.AutoSize = true;
	 this.cb_bersek.Location = new System.Drawing.Point(13, 15);
	 this.cb_bersek.Name = "cb_bersek";
	 this.cb_bersek.Size = new System.Drawing.Size(89, 17);
	 this.cb_bersek.TabIndex = 0;
	 this.cb_bersek.Text = "Bersek Mode";
	 this.cb_bersek.UseVisualStyleBackColor = true;
	 // 
	 // tabPage6
	 // 
	 this.tabPage6.BackColor = System.Drawing.Color.Gray;
	 this.tabPage6.Controls.Add(this.cb_autocurse);
	 this.tabPage6.Controls.Add(this.cb_sendpet);
	 this.tabPage6.Controls.Add(this.cb_wand);
	 this.tabPage6.Controls.Add(this.cb_drain_soul);
	 this.tabPage6.Controls.Add(this.cb_COA);
	 this.tabPage6.Controls.Add(this.label32);
	 this.tabPage6.Controls.Add(this.tb_pull_hp_lock);
	 this.tabPage6.Controls.Add(this.cb_lifetap_auto);
	 this.tabPage6.Controls.Add(this.tb_lifetap_hp);
	 this.tabPage6.Controls.Add(this.label31);
	 this.tabPage6.Controls.Add(this.tb_lifetap_mana);
	 this.tabPage6.Controls.Add(this.label30);
	 this.tabPage6.Controls.Add(this.cb_use_immolate);
	 this.tabPage6.Controls.Add(this.cb_use_corruption);
	 this.tabPage6.Controls.Add(this.cb_COW);
	 this.tabPage6.Controls.Add(this.tb_shadowbolt_mana);
	 this.tabPage6.Controls.Add(this.cb_use_shadowbolt);
	 this.tabPage6.Location = new System.Drawing.Point(4, 22);
	 this.tabPage6.Name = "tabPage6";
	 this.tabPage6.Size = new System.Drawing.Size(351, 347);
	 this.tabPage6.TabIndex = 3;
	 this.tabPage6.Text = "Warlock";
	 // 
	 // cb_autocurse
	 // 
	 this.cb_autocurse.AutoSize = true;
	 this.cb_autocurse.Checked = true;
	 this.cb_autocurse.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_autocurse.Location = new System.Drawing.Point(117, 53);
	 this.cb_autocurse.Name = "cb_autocurse";
	 this.cb_autocurse.Size = new System.Drawing.Size(48, 17);
	 this.cb_autocurse.TabIndex = 16;
	 this.cb_autocurse.Text = "Auto";
	 this.cb_autocurse.UseVisualStyleBackColor = true;
	 // 
	 // cb_sendpet
	 // 
	 this.cb_sendpet.AutoSize = true;
	 this.cb_sendpet.Location = new System.Drawing.Point(7, 209);
	 this.cb_sendpet.Name = "cb_sendpet";
	 this.cb_sendpet.Size = new System.Drawing.Size(70, 17);
	 this.cb_sendpet.TabIndex = 15;
	 this.cb_sendpet.Text = "Send Pet";
	 this.cb_sendpet.UseVisualStyleBackColor = true;
	 // 
	 // cb_wand
	 // 
	 this.cb_wand.AutoSize = true;
	 this.cb_wand.Location = new System.Drawing.Point(7, 190);
	 this.cb_wand.Name = "cb_wand";
	 this.cb_wand.Size = new System.Drawing.Size(77, 17);
	 this.cb_wand.TabIndex = 14;
	 this.cb_wand.Text = "Use Wand";
	 this.cb_wand.UseVisualStyleBackColor = true;
	 // 
	 // cb_drain_soul
	 // 
	 this.cb_drain_soul.AutoSize = true;
	 this.cb_drain_soul.Checked = true;
	 this.cb_drain_soul.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_drain_soul.Location = new System.Drawing.Point(7, 166);
	 this.cb_drain_soul.Name = "cb_drain_soul";
	 this.cb_drain_soul.Size = new System.Drawing.Size(75, 17);
	 this.cb_drain_soul.TabIndex = 13;
	 this.cb_drain_soul.Text = "Drain Soul";
	 this.cb_drain_soul.UseVisualStyleBackColor = true;
	 // 
	 // cb_COA
	 // 
	 this.cb_COA.AutoSize = true;
	 this.cb_COA.Location = new System.Drawing.Point(62, 53);
	 this.cb_COA.Name = "cb_COA";
	 this.cb_COA.Size = new System.Drawing.Size(48, 17);
	 this.cb_COA.TabIndex = 12;
	 this.cb_COA.Text = "COA";
	 this.cb_COA.UseVisualStyleBackColor = true;
	 this.cb_COA.CheckedChanged += new System.EventHandler(this.cb_COA_CheckedChanged);
	 // 
	 // label32
	 // 
	 this.label32.AutoSize = true;
	 this.label32.Location = new System.Drawing.Point(28, 144);
	 this.label32.Name = "label32";
	 this.label32.Size = new System.Drawing.Size(42, 13);
	 this.label32.TabIndex = 11;
	 this.label32.Text = "Pull HP";
	 this.label32.Click += new System.EventHandler(this.label32_Click);
	 // 
	 // tb_pull_hp_lock
	 // 
	 this.tb_pull_hp_lock.Location = new System.Drawing.Point(76, 141);
	 this.tb_pull_hp_lock.Name = "tb_pull_hp_lock";
	 this.tb_pull_hp_lock.Size = new System.Drawing.Size(24, 20);
	 this.tb_pull_hp_lock.TabIndex = 10;
	 this.tb_pull_hp_lock.Text = "70";
	 // 
	 // cb_lifetap_auto
	 // 
	 this.cb_lifetap_auto.AutoSize = true;
	 this.cb_lifetap_auto.Location = new System.Drawing.Point(214, 117);
	 this.cb_lifetap_auto.Name = "cb_lifetap_auto";
	 this.cb_lifetap_auto.Size = new System.Drawing.Size(48, 17);
	 this.cb_lifetap_auto.TabIndex = 9;
	 this.cb_lifetap_auto.Text = "Auto";
	 this.cb_lifetap_auto.UseVisualStyleBackColor = true;
	 // 
	 // tb_lifetap_hp
	 // 
	 this.tb_lifetap_hp.Location = new System.Drawing.Point(175, 114);
	 this.tb_lifetap_hp.Name = "tb_lifetap_hp";
	 this.tb_lifetap_hp.Size = new System.Drawing.Size(24, 20);
	 this.tb_lifetap_hp.TabIndex = 8;
	 this.tb_lifetap_hp.Text = "70";
	 // 
	 // label31
	 // 
	 this.label31.AutoSize = true;
	 this.label31.Location = new System.Drawing.Point(106, 118);
	 this.label31.Name = "label31";
	 this.label31.Size = new System.Drawing.Size(63, 13);
	 this.label31.TabIndex = 7;
	 this.label31.Text = "Life tap HP:";
	 // 
	 // tb_lifetap_mana
	 // 
	 this.tb_lifetap_mana.Location = new System.Drawing.Point(76, 114);
	 this.tb_lifetap_mana.Name = "tb_lifetap_mana";
	 this.tb_lifetap_mana.Size = new System.Drawing.Size(24, 20);
	 this.tb_lifetap_mana.TabIndex = 6;
	 this.tb_lifetap_mana.Text = "40";
	 // 
	 // label30
	 // 
	 this.label30.AutoSize = true;
	 this.label30.Location = new System.Drawing.Point(4, 117);
	 this.label30.Name = "label30";
	 this.label30.Size = new System.Drawing.Size(77, 13);
	 this.label30.TabIndex = 5;
	 this.label30.Text = "Life tap mana: ";
	 // 
	 // cb_use_immolate
	 // 
	 this.cb_use_immolate.AutoSize = true;
	 this.cb_use_immolate.Location = new System.Drawing.Point(4, 94);
	 this.cb_use_immolate.Name = "cb_use_immolate";
	 this.cb_use_immolate.Size = new System.Drawing.Size(90, 17);
	 this.cb_use_immolate.TabIndex = 4;
	 this.cb_use_immolate.Text = "Use Immolate";
	 this.cb_use_immolate.UseVisualStyleBackColor = true;
	 // 
	 // cb_use_corruption
	 // 
	 this.cb_use_corruption.AutoSize = true;
	 this.cb_use_corruption.Location = new System.Drawing.Point(4, 71);
	 this.cb_use_corruption.Name = "cb_use_corruption";
	 this.cb_use_corruption.Size = new System.Drawing.Size(96, 17);
	 this.cb_use_corruption.TabIndex = 3;
	 this.cb_use_corruption.Text = "Use Corruption";
	 this.cb_use_corruption.UseVisualStyleBackColor = true;
	 // 
	 // cb_COW
	 // 
	 this.cb_COW.AutoSize = true;
	 this.cb_COW.Checked = true;
	 this.cb_COW.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_COW.Location = new System.Drawing.Point(4, 53);
	 this.cb_COW.Name = "cb_COW";
	 this.cb_COW.Size = new System.Drawing.Size(52, 17);
	 this.cb_COW.TabIndex = 2;
	 this.cb_COW.Text = "COW";
	 this.cb_COW.UseVisualStyleBackColor = true;
	 this.cb_COW.CheckedChanged += new System.EventHandler(this.cb_COW_CheckedChanged);
	 // 
	 // tb_shadowbolt_mana
	 // 
	 this.tb_shadowbolt_mana.Location = new System.Drawing.Point(130, 27);
	 this.tb_shadowbolt_mana.Name = "tb_shadowbolt_mana";
	 this.tb_shadowbolt_mana.Size = new System.Drawing.Size(32, 20);
	 this.tb_shadowbolt_mana.TabIndex = 1;
	 this.tb_shadowbolt_mana.Text = "40";
	 // 
	 // cb_use_shadowbolt
	 // 
	 this.cb_use_shadowbolt.AutoSize = true;
	 this.cb_use_shadowbolt.Location = new System.Drawing.Point(4, 29);
	 this.cb_use_shadowbolt.Name = "cb_use_shadowbolt";
	 this.cb_use_shadowbolt.Size = new System.Drawing.Size(120, 17);
	 this.cb_use_shadowbolt.TabIndex = 0;
	 this.cb_use_shadowbolt.Text = "Use Shadow Bolt at";
	 this.cb_use_shadowbolt.UseVisualStyleBackColor = true;
	 // 
	 // tabPage7
	 // 
	 this.tabPage7.BackColor = System.Drawing.Color.Gray;
	 this.tabPage7.Controls.Add(this.cb_shielded_pull);
	 this.tabPage7.Controls.Add(this.cb_combat_pws);
	 this.tabPage7.Controls.Add(this.cb_shielded_smite);
	 this.tabPage7.Controls.Add(this.label36);
	 this.tabPage7.Controls.Add(this.tb_mana_pull_priest);
	 this.tabPage7.Controls.Add(this.tb_smitemana);
	 this.tabPage7.Controls.Add(this.cb_use_priest_wand);
	 this.tabPage7.Controls.Add(this.cb_usesmite);
	 this.tabPage7.Controls.Add(this.label35);
	 this.tabPage7.Controls.Add(this.tb_renewat);
	 this.tabPage7.Controls.Add(this.label33);
	 this.tabPage7.Controls.Add(this.tb_priest_combatheal);
	 this.tabPage7.Controls.Add(this.label34);
	 this.tabPage7.Controls.Add(this.tb_priest_pullheal);
	 this.tabPage7.Location = new System.Drawing.Point(4, 22);
	 this.tabPage7.Name = "tabPage7";
	 this.tabPage7.Size = new System.Drawing.Size(351, 347);
	 this.tabPage7.TabIndex = 4;
	 this.tabPage7.Text = "Priest";
	 // 
	 // cb_shielded_pull
	 // 
	 this.cb_shielded_pull.AutoSize = true;
	 this.cb_shielded_pull.Location = new System.Drawing.Point(179, 226);
	 this.cb_shielded_pull.Name = "cb_shielded_pull";
	 this.cb_shielded_pull.Size = new System.Drawing.Size(87, 17);
	 this.cb_shielded_pull.TabIndex = 90;
	 this.cb_shielded_pull.Text = "Shielded Pull";
	 this.cb_shielded_pull.UseVisualStyleBackColor = true;
	 // 
	 // cb_combat_pws
	 // 
	 this.cb_combat_pws.AutoSize = true;
	 this.cb_combat_pws.Checked = true;
	 this.cb_combat_pws.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_combat_pws.Location = new System.Drawing.Point(45, 226);
	 this.cb_combat_pws.Name = "cb_combat_pws";
	 this.cb_combat_pws.Size = new System.Drawing.Size(90, 17);
	 this.cb_combat_pws.TabIndex = 89;
	 this.cb_combat_pws.Text = "Combat PWS";
	 this.cb_combat_pws.UseVisualStyleBackColor = true;
	 // 
	 // cb_shielded_smite
	 // 
	 this.cb_shielded_smite.AutoSize = true;
	 this.cb_shielded_smite.Checked = true;
	 this.cb_shielded_smite.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_shielded_smite.Location = new System.Drawing.Point(163, 134);
	 this.cb_shielded_smite.Name = "cb_shielded_smite";
	 this.cb_shielded_smite.Size = new System.Drawing.Size(89, 17);
	 this.cb_shielded_smite.TabIndex = 88;
	 this.cb_shielded_smite.Text = "Only shielded";
	 this.cb_shielded_smite.UseVisualStyleBackColor = true;
	 // 
	 // label36
	 // 
	 this.label36.AutoSize = true;
	 this.label36.Location = new System.Drawing.Point(63, 193);
	 this.label36.Name = "label36";
	 this.label36.Size = new System.Drawing.Size(54, 13);
	 this.label36.TabIndex = 87;
	 this.label36.Text = "Pull Mana";
	 // 
	 // tb_mana_pull_priest
	 // 
	 this.tb_mana_pull_priest.Location = new System.Drawing.Point(123, 190);
	 this.tb_mana_pull_priest.Name = "tb_mana_pull_priest";
	 this.tb_mana_pull_priest.Size = new System.Drawing.Size(33, 20);
	 this.tb_mana_pull_priest.TabIndex = 86;
	 this.tb_mana_pull_priest.Text = "70";
	 // 
	 // tb_smitemana
	 // 
	 this.tb_smitemana.Location = new System.Drawing.Point(123, 132);
	 this.tb_smitemana.Name = "tb_smitemana";
	 this.tb_smitemana.Size = new System.Drawing.Size(33, 20);
	 this.tb_smitemana.TabIndex = 85;
	 this.tb_smitemana.Text = "40";
	 // 
	 // cb_use_priest_wand
	 // 
	 this.cb_use_priest_wand.AutoSize = true;
	 this.cb_use_priest_wand.Location = new System.Drawing.Point(45, 157);
	 this.cb_use_priest_wand.Name = "cb_use_priest_wand";
	 this.cb_use_priest_wand.Size = new System.Drawing.Size(77, 17);
	 this.cb_use_priest_wand.TabIndex = 84;
	 this.cb_use_priest_wand.Text = "Use Wand";
	 this.cb_use_priest_wand.UseVisualStyleBackColor = true;
	 // 
	 // cb_usesmite
	 // 
	 this.cb_usesmite.AutoSize = true;
	 this.cb_usesmite.Location = new System.Drawing.Point(45, 134);
	 this.cb_usesmite.Name = "cb_usesmite";
	 this.cb_usesmite.Size = new System.Drawing.Size(72, 17);
	 this.cb_usesmite.TabIndex = 83;
	 this.cb_usesmite.Text = "Use smite";
	 this.cb_usesmite.UseVisualStyleBackColor = true;
	 // 
	 // label35
	 // 
	 this.label35.AutoSize = true;
	 this.label35.Location = new System.Drawing.Point(42, 95);
	 this.label35.Name = "label35";
	 this.label35.Size = new System.Drawing.Size(53, 13);
	 this.label35.TabIndex = 82;
	 this.label35.Text = "Renew at";
	 // 
	 // tb_renewat
	 // 
	 this.tb_renewat.Location = new System.Drawing.Point(111, 91);
	 this.tb_renewat.Name = "tb_renewat";
	 this.tb_renewat.Size = new System.Drawing.Size(33, 20);
	 this.tb_renewat.TabIndex = 81;
	 this.tb_renewat.Text = "40";
	 // 
	 // label33
	 // 
	 this.label33.AutoSize = true;
	 this.label33.Location = new System.Drawing.Point(42, 20);
	 this.label33.Name = "label33";
	 this.label33.Size = new System.Drawing.Size(66, 13);
	 this.label33.TabIndex = 80;
	 this.label33.Text = "Combat heal";
	 // 
	 // tb_priest_combatheal
	 // 
	 this.tb_priest_combatheal.Location = new System.Drawing.Point(111, 17);
	 this.tb_priest_combatheal.Name = "tb_priest_combatheal";
	 this.tb_priest_combatheal.Size = new System.Drawing.Size(33, 20);
	 this.tb_priest_combatheal.TabIndex = 79;
	 this.tb_priest_combatheal.Text = "40";
	 // 
	 // label34
	 // 
	 this.label34.AutoSize = true;
	 this.label34.Location = new System.Drawing.Point(17, 61);
	 this.label34.Name = "label34";
	 this.label34.Size = new System.Drawing.Size(88, 13);
	 this.label34.TabIndex = 78;
	 this.label34.Text = "Pre-Combat heal ";
	 // 
	 // tb_priest_pullheal
	 // 
	 this.tb_priest_pullheal.Location = new System.Drawing.Point(111, 58);
	 this.tb_priest_pullheal.Name = "tb_priest_pullheal";
	 this.tb_priest_pullheal.Size = new System.Drawing.Size(33, 20);
	 this.tb_priest_pullheal.TabIndex = 77;
	 this.tb_priest_pullheal.Text = "70";
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
	 this.tabPage3.Size = new System.Drawing.Size(697, 502);
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
	 // Statistics
	 // 
	 this.Statistics.BackColor = System.Drawing.Color.DarkGray;
	 this.Statistics.Controls.Add(this.label46);
	 this.Statistics.Controls.Add(this.tb_kills_hunter);
	 this.Statistics.Controls.Add(this.label45);
	 this.Statistics.Controls.Add(this.label44);
	 this.Statistics.Controls.Add(this.tb_kills_mage);
	 this.Statistics.Controls.Add(this.tb_kills_druid);
	 this.Statistics.Controls.Add(this.tb_kills_rogue);
	 this.Statistics.Controls.Add(this.tb_kills_paladin);
	 this.Statistics.Controls.Add(this.tb_kills_warrior);
	 this.Statistics.Controls.Add(this.tb_kills_warlock);
	 this.Statistics.Controls.Add(this.tb_kills_priest);
	 this.Statistics.Controls.Add(this.label43);
	 this.Statistics.Controls.Add(this.label42);
	 this.Statistics.Controls.Add(this.label41);
	 this.Statistics.Controls.Add(this.label40);
	 this.Statistics.Controls.Add(this.label39);
	 this.Statistics.Location = new System.Drawing.Point(4, 22);
	 this.Statistics.Name = "Statistics";
	 this.Statistics.Size = new System.Drawing.Size(697, 502);
	 this.Statistics.TabIndex = 3;
	 this.Statistics.Text = "Statistics";
	 // 
	 // label46
	 // 
	 this.label46.AutoSize = true;
	 this.label46.Location = new System.Drawing.Point(41, 212);
	 this.label46.Name = "label46";
	 this.label46.Size = new System.Drawing.Size(39, 13);
	 this.label46.TabIndex = 15;
	 this.label46.Text = "Hunter";
	 // 
	 // tb_kills_hunter
	 // 
	 this.tb_kills_hunter.Enabled = false;
	 this.tb_kills_hunter.Location = new System.Drawing.Point(101, 209);
	 this.tb_kills_hunter.Name = "tb_kills_hunter";
	 this.tb_kills_hunter.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_hunter.TabIndex = 14;
	 this.tb_kills_hunter.Text = "0";
	 // 
	 // label45
	 // 
	 this.label45.AutoSize = true;
	 this.label45.Location = new System.Drawing.Point(41, 188);
	 this.label45.Name = "label45";
	 this.label45.Size = new System.Drawing.Size(34, 13);
	 this.label45.TabIndex = 13;
	 this.label45.Text = "Mage";
	 // 
	 // label44
	 // 
	 this.label44.AutoSize = true;
	 this.label44.Location = new System.Drawing.Point(39, 162);
	 this.label44.Name = "label44";
	 this.label44.Size = new System.Drawing.Size(32, 13);
	 this.label44.TabIndex = 12;
	 this.label44.Text = "Druid";
	 // 
	 // tb_kills_mage
	 // 
	 this.tb_kills_mage.Enabled = false;
	 this.tb_kills_mage.Location = new System.Drawing.Point(101, 185);
	 this.tb_kills_mage.Name = "tb_kills_mage";
	 this.tb_kills_mage.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_mage.TabIndex = 11;
	 this.tb_kills_mage.Text = "0";
	 // 
	 // tb_kills_druid
	 // 
	 this.tb_kills_druid.Enabled = false;
	 this.tb_kills_druid.Location = new System.Drawing.Point(101, 159);
	 this.tb_kills_druid.Name = "tb_kills_druid";
	 this.tb_kills_druid.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_druid.TabIndex = 10;
	 this.tb_kills_druid.Text = "0";
	 // 
	 // tb_kills_rogue
	 // 
	 this.tb_kills_rogue.Enabled = false;
	 this.tb_kills_rogue.Location = new System.Drawing.Point(101, 133);
	 this.tb_kills_rogue.Name = "tb_kills_rogue";
	 this.tb_kills_rogue.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_rogue.TabIndex = 9;
	 this.tb_kills_rogue.Text = "0";
	 // 
	 // tb_kills_paladin
	 // 
	 this.tb_kills_paladin.Enabled = false;
	 this.tb_kills_paladin.Location = new System.Drawing.Point(101, 109);
	 this.tb_kills_paladin.Name = "tb_kills_paladin";
	 this.tb_kills_paladin.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_paladin.TabIndex = 8;
	 this.tb_kills_paladin.Text = "0";
	 // 
	 // tb_kills_warrior
	 // 
	 this.tb_kills_warrior.Enabled = false;
	 this.tb_kills_warrior.Location = new System.Drawing.Point(101, 83);
	 this.tb_kills_warrior.Name = "tb_kills_warrior";
	 this.tb_kills_warrior.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_warrior.TabIndex = 7;
	 this.tb_kills_warrior.Text = "0";
	 // 
	 // tb_kills_warlock
	 // 
	 this.tb_kills_warlock.Enabled = false;
	 this.tb_kills_warlock.Location = new System.Drawing.Point(101, 57);
	 this.tb_kills_warlock.Name = "tb_kills_warlock";
	 this.tb_kills_warlock.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_warlock.TabIndex = 6;
	 this.tb_kills_warlock.Text = "0";
	 // 
	 // tb_kills_priest
	 // 
	 this.tb_kills_priest.Enabled = false;
	 this.tb_kills_priest.Location = new System.Drawing.Point(101, 35);
	 this.tb_kills_priest.Name = "tb_kills_priest";
	 this.tb_kills_priest.Size = new System.Drawing.Size(49, 20);
	 this.tb_kills_priest.TabIndex = 5;
	 this.tb_kills_priest.Text = "0";
	 // 
	 // label43
	 // 
	 this.label43.AutoSize = true;
	 this.label43.Location = new System.Drawing.Point(39, 136);
	 this.label43.Name = "label43";
	 this.label43.Size = new System.Drawing.Size(39, 13);
	 this.label43.TabIndex = 4;
	 this.label43.Text = "Rogue";
	 // 
	 // label42
	 // 
	 this.label42.AutoSize = true;
	 this.label42.Location = new System.Drawing.Point(38, 113);
	 this.label42.Name = "label42";
	 this.label42.Size = new System.Drawing.Size(42, 13);
	 this.label42.TabIndex = 3;
	 this.label42.Text = "Paladin";
	 // 
	 // label41
	 // 
	 this.label41.AutoSize = true;
	 this.label41.Location = new System.Drawing.Point(39, 86);
	 this.label41.Name = "label41";
	 this.label41.Size = new System.Drawing.Size(41, 13);
	 this.label41.TabIndex = 2;
	 this.label41.Text = "Warrior";
	 // 
	 // label40
	 // 
	 this.label40.AutoSize = true;
	 this.label40.Location = new System.Drawing.Point(39, 60);
	 this.label40.Name = "label40";
	 this.label40.Size = new System.Drawing.Size(47, 13);
	 this.label40.TabIndex = 1;
	 this.label40.Text = "Warlock";
	 // 
	 // label39
	 // 
	 this.label39.AutoSize = true;
	 this.label39.Location = new System.Drawing.Point(45, 38);
	 this.label39.Name = "label39";
	 this.label39.Size = new System.Drawing.Size(33, 13);
	 this.label39.TabIndex = 0;
	 this.label39.Text = "Priest";
	 // 
	 // label49
	 // 
	 this.label49.AutoSize = true;
	 this.label49.Location = new System.Drawing.Point(34, 485);
	 this.label49.Name = "label49";
	 this.label49.Size = new System.Drawing.Size(13, 13);
	 this.label49.TabIndex = 74;
	 this.label49.Text = "0";
	 // 
	 // label48
	 // 
	 this.label48.AutoSize = true;
	 this.label48.Location = new System.Drawing.Point(39, 510);
	 this.label48.Name = "label48";
	 this.label48.Size = new System.Drawing.Size(16, 13);
	 this.label48.TabIndex = 73;
	 this.label48.Text = "-1";
	 // 
	 // label47
	 // 
	 this.label47.AutoSize = true;
	 this.label47.Location = new System.Drawing.Point(36, 462);
	 this.label47.Name = "label47";
	 this.label47.Size = new System.Drawing.Size(19, 13);
	 this.label47.TabIndex = 72;
	 this.label47.Text = "+1";
	 // 
	 // label22
	 // 
	 this.label22.AutoSize = true;
	 this.label22.Location = new System.Drawing.Point(6, 436);
	 this.label22.Name = "label22";
	 this.label22.Size = new System.Drawing.Size(111, 13);
	 this.label22.TabIndex = 71;
	 this.label22.Text = "Damage / 100 energy";
	 // 
	 // tb_minus1
	 // 
	 this.tb_minus1.Location = new System.Drawing.Point(62, 507);
	 this.tb_minus1.Name = "tb_minus1";
	 this.tb_minus1.Size = new System.Drawing.Size(42, 20);
	 this.tb_minus1.TabIndex = 70;
	 // 
	 // tb_regular
	 // 
	 this.tb_regular.Location = new System.Drawing.Point(62, 485);
	 this.tb_regular.Name = "tb_regular";
	 this.tb_regular.Size = new System.Drawing.Size(42, 20);
	 this.tb_regular.TabIndex = 69;
	 // 
	 // tb_plus1
	 // 
	 this.tb_plus1.Location = new System.Drawing.Point(62, 459);
	 this.tb_plus1.Name = "tb_plus1";
	 this.tb_plus1.Size = new System.Drawing.Size(42, 20);
	 this.tb_plus1.TabIndex = 68;
	 // 
	 // tb_timer_hours
	 // 
	 this.tb_timer_hours.Location = new System.Drawing.Point(52, 381);
	 this.tb_timer_hours.Name = "tb_timer_hours";
	 this.tb_timer_hours.Size = new System.Drawing.Size(25, 20);
	 this.tb_timer_hours.TabIndex = 55;
	 this.tb_timer_hours.Text = "6";
	 this.tb_timer_hours.TextChanged += new System.EventHandler(this.tb_timer_hours_TextChanged);
	 // 
	 // cb_HS_timer
	 // 
	 this.cb_HS_timer.AutoSize = true;
	 this.cb_HS_timer.Checked = true;
	 this.cb_HS_timer.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_HS_timer.Location = new System.Drawing.Point(3, 383);
	 this.cb_HS_timer.Name = "cb_HS_timer";
	 this.cb_HS_timer.Size = new System.Drawing.Size(52, 17);
	 this.cb_HS_timer.TabIndex = 51;
	 this.cb_HS_timer.Text = "Timer";
	 this.cb_HS_timer.UseVisualStyleBackColor = true;
	 // 
	 // hs_min_left
	 // 
	 this.hs_min_left.Location = new System.Drawing.Point(8, 406);
	 this.hs_min_left.Name = "hs_min_left";
	 this.hs_min_left.Size = new System.Drawing.Size(93, 20);
	 this.hs_min_left.TabIndex = 50;
	 this.hs_min_left.Text = "300";
	 // 
	 // bt_onoff
	 // 
	 this.bt_onoff.Location = new System.Drawing.Point(5, 288);
	 this.bt_onoff.Name = "bt_onoff";
	 this.bt_onoff.Size = new System.Drawing.Size(75, 23);
	 this.bt_onoff.TabIndex = 0;
	 this.bt_onoff.Text = "Stop";
	 this.bt_onoff.UseVisualStyleBackColor = true;
	 this.bt_onoff.Click += new System.EventHandler(this.button1_Click);
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
	 // tb_debug1
	 // 
	 this.tb_debug1.Location = new System.Drawing.Point(3, 348);
	 this.tb_debug1.Name = "tb_debug1";
	 this.tb_debug1.Size = new System.Drawing.Size(50, 20);
	 this.tb_debug1.TabIndex = 11;
	 this.tb_debug1.Text = "5581";
	 // 
	 // tb_debug2
	 // 
	 this.tb_debug2.Location = new System.Drawing.Point(59, 348);
	 this.tb_debug2.Name = "tb_debug2";
	 this.tb_debug2.Size = new System.Drawing.Size(41, 20);
	 this.tb_debug2.TabIndex = 10;
	 this.tb_debug2.Text = "6099";
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
	 this.lb_delta.Location = new System.Drawing.Point(86, 319);
	 this.lb_delta.Name = "lb_delta";
	 this.lb_delta.Size = new System.Drawing.Size(13, 13);
	 this.lb_delta.TabIndex = 26;
	 this.lb_delta.Text = "0";
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
	 // cb_humanlike
	 // 
	 this.cb_humanlike.AutoSize = true;
	 this.cb_humanlike.Checked = true;
	 this.cb_humanlike.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_humanlike.Location = new System.Drawing.Point(9, 4);
	 this.cb_humanlike.Name = "cb_humanlike";
	 this.cb_humanlike.Size = new System.Drawing.Size(83, 17);
	 this.cb_humanlike.TabIndex = 66;
	 this.cb_humanlike.Text = "Human Like";
	 this.cb_humanlike.UseVisualStyleBackColor = true;
	 // 
	 // cb_cloudemove
	 // 
	 this.cb_cloudemove.AutoSize = true;
	 this.cb_cloudemove.Checked = true;
	 this.cb_cloudemove.CheckState = System.Windows.Forms.CheckState.Checked;
	 this.cb_cloudemove.Location = new System.Drawing.Point(49, 22);
	 this.cb_cloudemove.Name = "cb_cloudemove";
	 this.cb_cloudemove.Size = new System.Drawing.Size(59, 17);
	 this.cb_cloudemove.TabIndex = 67;
	 this.cb_cloudemove.Text = "Claude";
	 this.cb_cloudemove.UseVisualStyleBackColor = true;
	 // 
	 // tb_prob_plus1
	 // 
	 this.tb_prob_plus1.Location = new System.Drawing.Point(5, 455);
	 this.tb_prob_plus1.Name = "tb_prob_plus1";
	 this.tb_prob_plus1.Size = new System.Drawing.Size(25, 20);
	 this.tb_prob_plus1.TabIndex = 75;
	 this.tb_prob_plus1.Text = "33";
	 // 
	 // tb_prob_zero
	 // 
	 this.tb_prob_zero.Location = new System.Drawing.Point(5, 478);
	 this.tb_prob_zero.Name = "tb_prob_zero";
	 this.tb_prob_zero.Size = new System.Drawing.Size(25, 20);
	 this.tb_prob_zero.TabIndex = 76;
	 this.tb_prob_zero.Text = "34";
	 // 
	 // tb_prob_minus1
	 // 
	 this.tb_prob_minus1.Location = new System.Drawing.Point(5, 503);
	 this.tb_prob_minus1.Name = "tb_prob_minus1";
	 this.tb_prob_minus1.Size = new System.Drawing.Size(25, 20);
	 this.tb_prob_minus1.TabIndex = 77;
	 this.tb_prob_minus1.Text = "33";
	 // 
	 // tabPage8
	 // 
	 this.tabPage8.BackColor = System.Drawing.Color.DarkGray;
	 this.tabPage8.ForeColor = System.Drawing.SystemColors.ButtonShadow;
	 this.tabPage8.Location = new System.Drawing.Point(4, 22);
	 this.tabPage8.Name = "tabPage8";
	 this.tabPage8.Size = new System.Drawing.Size(351, 347);
	 this.tabPage8.TabIndex = 5;
	 this.tabPage8.Text = "Hunter";
	 // 
	 // Form1
	 // 
	 this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
	 this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	 this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
	 this.ClientSize = new System.Drawing.Size(819, 544);
	 this.Controls.Add(this.tb_prob_minus1);
	 this.Controls.Add(this.tb_prob_zero);
	 this.Controls.Add(this.tb_prob_plus1);
	 this.Controls.Add(this.label49);
	 this.Controls.Add(this.cb_cloudemove);
	 this.Controls.Add(this.cb_humanlike);
	 this.Controls.Add(this.label48);
	 this.Controls.Add(this.button19);
	 this.Controls.Add(this.label47);
	 this.Controls.Add(this.button16);
	 this.Controls.Add(this.button12);
	 this.Controls.Add(this.label22);
	 this.Controls.Add(this.tb_timer_hours);
	 this.Controls.Add(this.tb_minus1);
	 this.Controls.Add(this.lb_delta);
	 this.Controls.Add(this.label15);
	 this.Controls.Add(this.tb_regular);
	 this.Controls.Add(this.label14);
	 this.Controls.Add(this.bt_save_cfg);
	 this.Controls.Add(this.tb_plus1);
	 this.Controls.Add(this.hs_min_left);
	 this.Controls.Add(this.cb_HS_timer);
	 this.Controls.Add(this.bt_debug2);
	 this.Controls.Add(this.bt_anda);
	 this.Controls.Add(this.tb_debug1);
	 this.Controls.Add(this.tb_debug2);
	 this.Controls.Add(this.bt_getstats);
	 this.Controls.Add(this.bt_loadWP);
	 this.Controls.Add(this.tb_yaw);
	 this.Controls.Add(this.tb_spd);
	 this.Controls.Add(this.tb_y);
	 this.Controls.Add(this.tb_x);
	 this.Controls.Add(this.tab_nav);
	 this.Controls.Add(this.bt_onoff);
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
	 this.tabPage4.ResumeLayout(false);
	 this.tabPage4.PerformLayout();
	 this.Paladin.ResumeLayout(false);
	 this.Paladin.PerformLayout();
	 this.tabPage5.ResumeLayout(false);
	 this.tabPage5.PerformLayout();
	 this.tabPage6.ResumeLayout(false);
	 this.tabPage6.PerformLayout();
	 this.tabPage7.ResumeLayout(false);
	 this.tabPage7.PerformLayout();
	 this.pan_tar.ResumeLayout(false);
	 this.pan_tar.PerformLayout();
	 this.pan_me.ResumeLayout(false);
	 this.pan_me.PerformLayout();
	 this.tabPage3.ResumeLayout(false);
	 this.tabPage3.PerformLayout();
	 this.Statistics.ResumeLayout(false);
	 this.Statistics.PerformLayout();
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
	private System.Windows.Forms.TextBox tb_debug1;
	private System.Windows.Forms.TextBox tb_debug2;
	private System.Windows.Forms.Button bt_debug2;
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
	private System.Windows.Forms.CheckBox cb_concentration_aura;
	private System.Windows.Forms.PictureBox pb_minimap;
	private System.Windows.Forms.TextBox tbraio;
	private System.Windows.Forms.TextBox tby;
	private System.Windows.Forms.TextBox tbx;
	private System.Windows.Forms.Button button10;
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
	private CheckBox cb_pickpocket;
	private CheckBox cb_stealth_pull;
	private TextBox tb_evasion;
	private Label label26;
	private CheckBox cb_noelite;
	private TabPage tabPage5;
	private CheckBox cb_bersek;
	private CheckBox cb_war_rangepull;
	private CheckBox cb_use_thunderclap;
	private TextBox tb_thunderclap_count;
	private TextBox tb_heroic_strike_rage;
	private Label label27;
	private CheckBox cb_use_rend;
	private TextBox tb_bloorrage_maxmobs;
	private CheckBox cb_use_bloodrage;
	private CheckBox cb_range_pull;
	private CheckBox cb_evis_auto;
	private CheckBox cb_SAD;
	private CheckBox cb_expose_armor;
	private Label label28;
	private Button button21;
	private CheckBox cb_humanlike;
	private TextBox tb_rest_warr;
	private Label label29;
	private CheckBox cb_nomech;
	private CheckBox cb_cloudemove;
	private CheckBox cb_randomize_rogue;
	private TextBox tb_mouse_drag;
	private CheckBox cb_random_pull_warrior;
	private Button bt_fenceA;
	private Button bt_savefence;
	private Button bt_fenceB;
	protected Button bt_fenceP;
	private TabPage tabPage6;
	private CheckBox cb_COW;
	private TextBox tb_shadowbolt_mana;
	private CheckBox cb_use_shadowbolt;
	private CheckBox cb_use_corruption;
	private CheckBox cb_use_immolate;
	private TextBox tb_lifetap_hp;
	private Label label31;
	private TextBox tb_lifetap_mana;
	private Label label30;
	private CheckBox cb_lifetap_auto;
	private Label label32;
	private TextBox tb_pull_hp_lock;
	private CheckBox cb_COA;
	private CheckBox cb_drain_soul;
	private CheckBox cb_wand;
	private CheckBox cb_sendpet;
	private CheckBox cb_autocurse;
	private TabPage tabPage7;
	private Label label33;
	private TextBox tb_priest_combatheal;
	private Label label34;
	private TextBox tb_priest_pullheal;
	private Label label35;
	private TextBox tb_renewat;
	private TextBox tb_smitemana;
	private CheckBox cb_use_priest_wand;
	private CheckBox cb_usesmite;
	private Label label36;
	private TextBox tb_mana_pull_priest;
	private Label label37;
	private TextBox lb_combatlog;
	private Label label38;
	private TabPage Statistics;
	private Label label43;
	private Label label42;
	private Label label41;
	private Label label40;
	private Label label39;
	private TextBox tb_kills_rogue;
	private TextBox tb_kills_paladin;
	private TextBox tb_kills_warrior;
	private TextBox tb_kills_warlock;
	private TextBox tb_kills_priest;
	private Label label45;
	private Label label44;
	private TextBox tb_kills_mage;
	private TextBox tb_kills_druid;
	private Label label46;
	private TextBox tb_kills_hunter;
	private CheckBox cb_shielded_smite;
	private CheckBox cb_shielded_pull;
	private CheckBox cb_combat_pws;
	private TextBox tb_debug;
	private TextBox tb_plus1;
	private TextBox tb_regular;
	private TextBox tb_minus1;
	private Label label22;
	private Label label47;
	private Label label48;
	private Label label49;
	private Label label50;
	private TextBox tb_energy_ss;
	private TextBox tb_prob_plus1;
	private TextBox tb_prob_zero;
	private TextBox tb_prob_minus1;
	private CheckBox cb_no_backpedal;
	private CheckBox cb_scan_highlevel;
	private CheckBox elemental_patrol;
	private TextBox tb_hearthlevel;
	private CheckBox cb_hearth_ding;
	private CheckBox cb_nomurloc;
	private Label label52;
	private TextBox tb_damage_ss;
	private Label label51;
	private TextBox tb_damage_hit;
	private CheckBox cb_sunderspam;
	private TabPage tabPage8;
 }
}

