using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Discord.Form1;

namespace Discord
{

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

	// ----------- específicos de mim (me) -----------
	public int freeslots;        // slots livres na bag
	public bool racialready;     // racial pronta
	public bool hp_potion_rdy;   // healing potion pronta
	public bool haspoison;       // envenenado
	public bool hasmagic;        // afetado por magia
	public bool hascurse;        // amaldiçoado
	public bool hasdisease;      // doente
	public bool hasother;        // outros debuffs (ex: bleed)
	public bool furbolg_form;    // está transformado com Dartol's Rod
	public bool dazed;           // está atordoado (dazed)

	// ----------- específicos do target (tar) -----------
	public bool aggroed;         // tem aggro em mim
	public bool israre;          // criatura rara (bit 64 no 7G)
	public bool ieslite;         // criatura elite (bit 32 no 7G)
															 

	// ----------- casting -----------
	public bool iscaster;        // é uma criatura do tipo caster (futuro)
	public bool casting;         // está castando
	public string spell;         // letra inicial ou nome parcial da spell
	public int castbar;          // progresso da castbar (0–100)
	public int spellid;          // ID real da spell (uso futuro)

	// ----------- uso futuro -----------
	public bool meleerange;      // está em alcance melee (futuro)
	public bool rangedrange;     // está em alcance à distância (futuro)
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
