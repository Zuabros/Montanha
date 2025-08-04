using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Discord.Form1;


namespace Discord
{

 public struct flag
 {
	public int wandGCD;
	public bool trinket1_up; // trinket 1 pronto (bit 0)
 }
 

 // --------------------------------
 // MÓDULO 12 - STRUCT ELEMENT
 // Representa qualquer entidade (player, mob, NPC) no mapa.
 // --------------------------------
 public struct element
 {
	// ----------- comuns ------------
	public loc pos;           // posição atual
	public int hp;            // vida (0–100%)
	public int mana;          // mana (0–100%)
	public int spd;           // velocidade atual (invertida no canal 1B)
	public int facing;        // direção que está olhando
	public int level;         // nível
	public int type;          // tipo (1 = player, 2 = mob, etc.)
	public bool combat;       // está em combate
	public bool morreu;       // está morto
	public int mood;          // -1 = hostil, 0 = neutro, 1 = amigável
	public bool humanoid;     // é humanóide
	public int classe;        // classe (1 = warrior, etc.)
	public bool hastarget;    // possui target
	public bool autoattack;   // autoattack ativo
	public bool wrongway;     // está de costas para o alvo
	public bool outofrange;   // alvo fora de alcance (melee ou spell)
	public bool swim;         // está nadando
	public bool armabroken;   // MH ou OH quebrada (bit 1 no 7G)
	
	// ----------- específicos de mim (me) -----------
	public int freeslots;        // slots livres na bag
	public bool racialready;     // racial pronta
	public bool hp_potion_rdy;   // healing potion pronta
	public bool haspoison;       // envenenado
	public bool hasmagic;        // afetado por magia
	public bool hascurse;        // amaldiçoado
	public bool hasdisease;      // doente
	public bool hasother;        // outros debuffs (ex: bleed)
	public bool dazed;           // está atordoado (dazed)
	public int  mobs;        // contador de mobs atacando
	public int mobs_player;        // contador de mobs atacando player
	public int mobs_pet;        // contador de mobs atacando pet 
	public bool  eating;        // comendo ou bebendo
	public bool ready;    // GCD pronto (Global Cooldown)
	public bool ranged;
	public bool iscaster;        // é uma criatura do tipo caster (MAGE, LOCK, PRIEST, etc.)
	public bool wandon; // wand ativa 1B7
	public bool wand_up; // wand ativa 1B7
	public bool bandage_up; // bandagem pronta (1B7)


	// ----------- específicos do target (tar) -----------
	public bool player_aggro;        // Player tem aggro (mob atacando player) - bit 0
	public bool pet_aggro;           // Pet tem aggro (mob atacando pet) - bit 1
	public bool israre;          // criatura rara (bit 64 no 7G)
	public bool iselite;         // criatura elite (bit 32 no 7G)
	public bool trivial;        // criatura trivial  
	public int id;            // ID do mob (usado para spells, etc.)

	// ----------- casting -----------
	
	public bool casting;         // está castando
	public int castbar;          // progresso da castbar (0–100)
	public int spellid;          // ID real da spell (uso futuro)

	// ----------- uso futuro -----------
	public bool melee;      // está em alcance melee (futuro)
	public bool rangedrange;     // está em alcance à distância (futuro)
	
 }
 // --------------------------------
 // CLASSE LOCKTABLE - STATUS DO WARLOCK
 // --------------------------------
 public class locktable
 {
	// SPELLS PRONTAS (true se cooldown + range + mana OK)
	public bool immolate_up;         // Immolate pronta para usar
	public bool corruption_up;       // Corruption pronta para usar  
	public bool curse_weakness_up;   // Curse of Weakness pronta para usar
	public bool curse_agony_up;      // Curse of Agony pronta para usar
	public bool drain_soul_up;       // Drain Soul pronta para usar
	public bool drain_life_up;       // Drain Life pronta para usar
	public bool siphon_life_up;      // Siphon Life pronta para usar
	public bool lifetap_up;   // life tap pronto 
	public bool shadowbolt_up; // Shadow Bolt pronto para usar
	public int shards;          // número de soul shards disponíveis (0–3+)
	public bool healhfunnel_up; // Heal ou Health Funnel pronto para usar
	public bool healthstone_up; // Healthstone pronto para usar
	public bool has_healthstone; // Healthstone pronto para usar
	public bool create_healthstone_up; // Create Healthstone pronto para usar
	


	// DEBUFFS NO TARGET (true se ativo)
	public bool has_immolate;        // Immolate ativo no target (>1s)
	public bool has_corruption;      // Corruption ativo no target (>1s)
	public bool has_curse_weakness;  // Curse of Weakness ativo no target
	public bool has_curse_agony;     // Curse of Agony ativo no target
	public bool has_drain_soul;      // Drain Soul sendo channelado
	public bool has_drain_life;      // Drain Life sendo channelado
	public bool has_siphon_life;     // Siphon Life ativo no target

	// PET STATUS
	public bool has_pet;             // Pet vivo e ativo
	public int pet_hp; // HP do pet (0–100%)

	// AURAS
	public bool has_demon_skin;      // bit 0 - Demon Skin ativo
 }
 // --------------------------------
 // CLASSE huntertable - STATUS DO HUNTER (VERSÃO ATUALIZADA)
 // --------------------------------
 public class huntertable
 {
	// ================================
	// SPELLS PRONTAS (true se cooldown + range + mana OK)
	// ================================
	public bool raptor_strike_up;         // Raptor Strike pronto (mana + range - como Heroic Strike)
	public bool auto_shot_up;             // Auto Shot pronto e em range
	public bool auto_shot_range_ok;       // Auto Shot range ok (verificação específica)

	public bool deterrence_up;          // Deterrence pronto  
	public bool serpent_sting_up;         // Serpent Sting pronto (spellready + manarange)
	public bool concussive_shot_up;       // Concussive Shot pronto (spellready + manarange)
	public bool arcane_shot_up;           // Arcane Shot pronto (spellready + manarange)
	public bool revive_pet_up;            // Revive Pet pronto (spellready + manarange)
	public bool disengage_up;             // Disengage pronto (spellready + manarange) 
	public bool aspect_monkey;
	public bool aspect_cheetah;
	public bool feigndeath_up;
	public bool rapidfire_up;
	public bool multishot_up;
	public bool bestialwrath_up;
	public bool raptor_queued; // captor strike aguardando 
	public bool intimidation_up; // Intimidation pronto (spellready + manarange)
	// ================================
	// STATUS ATIVO (true se ativo)
	// ================================
	public bool auto_shot_ativo;          // Auto Shot ativo (como autoattack)
	public bool feign_death_ativo;      // Feign Death ativo (como autoattack)

	// ================================
	// DEBUFFS NO TARGET
	// ================================
	public bool tar_serpent;              // Target tem debuff Serpent Sting

	// ================================
	// PET STATUS (como Warlock - específico da classe)
	// ================================
	public bool has_pet;                  // Pet vivo e ativo (como wlock.has_pet)
	public int pet_hp;                    // HP do pet (0-100%) (como wlock.pet_hp)
																				// NOTA: tar.pet_aggro é genérico e já existe no element
	public bool growl_autocast;
	public bool growl_available;
	public bool mongoose_bite_up;         // Mongoose Bite pronto (spellready + manarange)

 }


 // --------------------------------
 // CLASSE priestable
 // --------------------------------
 public class priestable
 {
	public bool has_pws;
	public bool has_renew;
	public bool has_fortitude;
	public bool has_innerfire;
	public bool has_innerfocus;
	public bool has_swp;
	public bool has_weakened_soul;
	public bool has_divine_spirit;
	public bool swp_up;
	public bool innerfocus_up;
	public bool fortitude_up;
	public bool innerfire_up;
	public bool renew_up;
	public bool pws_up;
	public bool smite_up;     // NOVO
	public bool heal_up;      // NOVO
	public bool divine_spirit_up;
	public bool desperate_prayer_up; // NOVO
 }
 // --------------------------------
 // CLASSE warriortable
 // --------------------------------
 public class warriortable
 {
	public bool rend_up;             // pode aplicar Rend?  
	public bool hs_up;               // pode usar Heroic Strike?  
	public bool cleave_up;           // pode usar Cleave?  
	public bool overpower_up;        // pode usar Overpower?  
	public bool bs_up;               // pode usar Battle Shout?  
	public bool bloodrage_up;        // pode usar Bloodrage?  
	public bool demo_up;             // pode usar Demoralizing Shout?  
	public bool thun_up;             // pode usar Thunder Clap?  
	public bool hams_up;             // pode usar Hamstring?  
	public bool retaliation_up;      // pode usar Retaliation?  
	public bool dwish_up;            // pode usar Death Wish?  
	public bool charge_up;           // pode usar Charge?  
	public bool throw_up;            // pode tacar throw knife (range, cooldown, etc.)
	public bool execute_up;          // pode usar Execute?
	public bool sunder_up;           // pode usar Sunder Armor (range + rage)?
	public bool slam_up;             // pode usar Slam? (range + rage)?
	public bool sweep_up;            // pode usar Sweeping Strikes? 
	public bool sweeping;            // Sweeping Strikes ativo?
	public bool revenge_up;          // pode usar Revenge?
	public bool shield_block_up;     // pode usar Shield Block e não tem aura ativa?
	public bool shield_wall_ready;   // pode usar Shield Wall?
	public bool stance;              // stance atual (0 = Battle, 1 = Defensive)
	public bool shield_bash_ready;   // pode usar Shield Bash?
	public bool disarm_ready;        // pode usar Disarm?
	public bool has_rend;            // target tem Rend ativo?  
	public bool has_demoralizing;    // target tem Demoralizing Shout ativo?
	public bool has_cleave;          // Cleave ativado?  
	public bool has_bs;              // Battle Shout ativo?
	public bool hs_casting;          // Heroic strike foi ativado?
	public bool revenge_proc;        // Revenge com (proc ativo)
	public bool has_shield_or_offhand;        // Tenho shield equipado
 }

 // --------------------------------
 // CLASSE roguetable
 // --------------------------------
 public class roguetable
 {
	public int energy;                  // energia atual do rogue (0–100)
	public int combo;                   // combo points acumulados (0–5, até 7 com talentos)
	public bool ss_up;                  // pode castar SS (range, cooldown, etc.)
	public bool evis_up;                // pode castar Eviscerate (range, cooldown, etc.)
	public bool throw_up;              // pode tacar faca (range, cooldown, etc.)
	public bool stealth_up;            // pode ativar Stealth agora
	public bool stealth;               // está em Stealth?
	public bool evasion_up;            // Evasion pronto para uso
	public bool SAD_up;                // Slice and Dice pronto
	public bool has_SAD;               // aura Slice and Dice ativa
	public bool expose_armor_up;       // Expose Armor pronto (energia, range, cooldown)
	public bool has_expose_armor;      // target com debuff Expose Armor
	public bool kick_up;               // Kick pronto
	public bool vanish_up;             // Vanish pronto
	public bool riposte_up;            // Riposte pronto
	public bool gouge_up;              // Gouge pronto
	public bool rupture_up;            // Rupture pronto
	public bool has_rupture;           // target com debuff Rupture
 }


 // --------------------------------
 // CLASSE PALATABLE - STATUS DO PALADINO
 // --------------------------------
 // --------------------------------
 // CLASSE PALATABLE
 // --------------------------------
 public class palatable
 {
	// AURAS (true se ativa)
	public bool devotion;        // Aura de armadura (bit 2)
	public bool retribution;     // Aura de dano reflexivo (bit 7)
	public bool concentration;   // Aura anti-pushback (bit 6)
	public bool fire;            // Aura contra dano de Fogo (bit 5)
	public bool frost;           // Aura contra dano de Gelo (bit 3)
	public bool shadow;          // Aura contra dano Sombrio (bit 4)
	public bool crusader;        // Crusader Aura (bit 1)

	// BLESSINGS (true se ativo)
	public bool bom;             // Blessing of Might
	public bool bow;             // Blessing of Wisdom
	public bool bok;             // Blessing of Kings
	public bool bos;             // Blessing of Salvation
	public bool bol;             // Blessing of Light
	public bool bof;             // Blessing of Freedom
	public bool bop;             // Blessing of Protection
	public bool bosanc;          // Blessing of Sanctuary

	// SEALS (true se ativo)
	public bool sor;             // Seal of Righteousness
	public bool soc;             // Seal of Command
	public bool sow;             // Seal of Wisdom
	public bool sol;             // Seal of Light
	public bool sotc;            // Seal of the Crusader
	public bool soj;            // Seal of the Crusader

	// COOLDOWNS (0–255s ou flags)
	public int judge_cd;         // Judgement
	public int dp_cd;            // Divine Protection
	
	// CAST READY 
	public bool exorcism_up;     // Exorcism disponível?
	public bool BOP_up;     // BOP disponível?
	public bool forbearance;            // debuff up
	public bool cancast_LOH;           // Lay on Hands
	public bool divine_protection_up;       // Divine Protection pronto?
	public bool hoj_ready;       // Hammer of Justice pronto?
	public bool divine_shield_up; // Divine Shield pronto?

	// TARGET DEBUFFS (true se ativo)
	public bool joj;             // Judgement of Justice
	public bool jol;             // Judgement of Light
	public bool jow;             // Judgement of Wisdom
	public bool jotc;            // Judgement of the Crusader

	// RANGE FLAGS (true se em alcance)
	public bool jud_range;       // Alcance para usar Judgement
	public bool hoj_range;       // Alcance para usar Hammer of Justice
	public bool exorcism_range;  // Alcance para usar Exorcism

	// FLAGS DE MODO DEFENSIVO
	public bool defseal;         // Está usando Seal defensivo?
	public bool defbless;        // Está usando Blessing defensiva?
	public bool defaura;         // Está usando Aura defensiva?
	public bool nomana;          // Mana abaixo de 20% no meio do combate
 }



 // --------------------------------
 // STRUCT POINT
 // Representa um ponto no mapa com posição real e código lógico
 // --------------------------------
 public struct point
 {
	public loc pos;    // posição no mundo real
	public byte val;   // tipo lógico (ex: 1 = player, 2 = mob, 9 = parede)

	public point(loc p, byte v)
	{
	 pos = p;   // atribui a posição
	 val = v;   // atribui o tipo
	}
 }
}
