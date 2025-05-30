using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Discord.Form1;

namespace Discord
{

 // --------------------------------
 // MÓDULO 12 - STRUCT ELEMENT - REPRESENTA QUALQUER ENTIDADE (PLAYER, MOB, NPC) NO MAPA.
 // --------------------------------
 public struct element
 {
	public loc pos;         // Posição atual
	public int hp;          // Vida (0–100%)
	public int mana;        // Mana (0–100%)
	public int spd;         // Velocidade atual
	public int facing;   // Direção que está olhando
	public int level;       // Nível
	public int type;        // Tipo (1 = player, 2 = mob, etc.)
	public bool combat;     // Está em combate
	public bool morreu;     // Está morto
	public int mood;        // -1 = hostil, 0 = neutro, 1 = amigável
	public bool humanoid;   // É humanóide
	public int classe;
	public bool hastarget;
	public bool autoattack; // on
	public int freeslots; // slots livres na bag 
	public bool haspoison;
	public bool hasmagic;
	public bool hascurse;
	public bool hasdisease;
	public bool hasother; //(bleed?)
	public bool racialready;  // racial up? 
	public bool wrongway; // facing 
	public bool outofrange; // da spell ou corporeo
	public bool swim; // nadando 
	public bool aggroed; // aggro em mim
	public bool skinnable; // da pra skinar (pixel 7)
	public bool furbolg_form; // Dartol's Rod of Transformation buff


	// --- casting ---
	public bool iscaster;   // É uma criatura do tipo caster      // uso futuro
	public bool casting;    // Está castando
	public string spell;    // Letra inicial ou nome parcial da spell
	public int castbar;     // Progresso da castbar (0–100)
	public int spellid;     // ID real da spell (futuro)

	// --- uso futuro ---
	public bool meleerange;   // Está em alcance melee           // uso futuro
	public bool rangedrange;  // Está em alcance à distância     // uso futuro
	public bool trivial;      // Mob trivial (baixo nível)       // uso futuro
	public bool lifeless;     // É um cadáver ou morto-vivo      // uso futuro
	public int mobcount;      // Número de mobs ao redor         // uso futuro

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
	public bool hoj_ready;       // Hammer of Justice pronto?
	public int judge_cd;         // Judgement
	public bool cancast_LOH;           // Lay on Hands
	public bool bubble_cd;       // Divine Shield (bolha)
	public int dp_cd;            // Divine Protection
	public bool exorcism_up;     // Exorcism disponível?
	public bool BOP_up;     // BOP disponível?
	public bool forbearance;            // debuff up

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
