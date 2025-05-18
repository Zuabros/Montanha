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
	public loc corpse;        // Posição do corpo/cadáver        // uso futuro
 }



 // --------------------------------
 // CLASSE PALATABLE - STATUS DO PALADINO
 // --------------------------------
 public class palatable
 {
	// AURAS (eternas - true se ativa)
	public bool devotion;        // Aura de armadura
	public bool retribution;     // Aura de dano reflexivo
	public bool concentration;   // Aura anti-pushback
	public bool fire;            // Aura contra dano de Fogo
	public bool frost;           // Aura contra dano de Gelo
	public bool shadow;          // Aura contra dano Sombrio
	public bool sanctity;        // Aura de dano sagrado aumentado

	// BLESSINGS (true se ativo)
	public bool bom;              // Blessing of Might
	public bool bow;              // Blessing of Wisdom
	public bool bok;              // Blessing of Kings
	public bool bos;              // Blessing of Salvation
	public bool bol;              // Blessing of Light
	public bool bof;              // Blessing of Freedom
	public bool bop;              // Blessing of Protection
																// SEALS (0–255s)
	public int sor;              // Seal of Righteousness
	public int soc;              // Seal of Command
	public int sow;              // Seal of Wisdom
	public int sol;              // Seal of Light
	public int sotc;             // Seal of the Crusader

	// COOLDOWNS (0–255s)
	public int hoj_cd;           // Hammer of Justice
	public int judge_cd;         // Judgement
	public int lay_cd;           // Lay on Hands
	public bool bubble_cd;        // Divine Shield (bolha)
	public int dp_cd;            // Divine Protection
	public int loh_cd;  // lay on hands
	public bool exorcism_up; // exorcism 

	// TARGET DEBUFFS:
	public bool joj;        // Judgement of justice 

	// RANGE FLAGS (true se em alcance)
	public bool jud_range;       // Alcance para usar Judgement
	public bool hoj_range;       // Alcance para usar Hammer of Justice
	public bool exorcism_range;       // Alcance para usar Exorcism
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
