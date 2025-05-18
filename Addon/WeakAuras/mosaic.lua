-- --------------------------------
-- addon mosaic v2.8 (Paladino)
-- --------------------------------

local largura = GetScreenWidth()
local altura = GetScreenHeight()
local base_y = 1
local passo = 4
local altura_pixel = 3
local altura_marcador = 1
local cor_marcador = {r = 230/255, g = 240/255, b = 250/255}

local pixels = {}
local marcadores = {}

-- cria os pixels e pratos
for i = 0, 15 do
    local f = CreateFrame("Frame", nil, UIParent)
    f:SetPoint("BOTTOMLEFT", UIParent, "BOTTOMLEFT", 0, base_y + i * passo)
    f:SetSize(5, altura_pixel)
    local tex = f:CreateTexture(nil, "BACKGROUND")
    tex:SetAllPoints()
    f.texture = tex
    f:SetFrameStrata("TOOLTIP")
    pixels[i] = f

    local marcador = CreateFrame("Frame", nil, UIParent)
    marcador:SetPoint("BOTTOMLEFT", f, "BOTTOMLEFT", 0, -altura_marcador)
    marcador:SetSize(5, altura_marcador)
    local tex_marcador = marcador:CreateTexture(nil, "BACKGROUND")
    tex_marcador:SetAllPoints()
    tex_marcador:SetColorTexture(cor_marcador.r, cor_marcador.g, cor_marcador.b)
    marcador.texture = tex_marcador
    marcador:SetFrameStrata("TOOLTIP")
    marcadores[i] = marcador
end

local tampa = CreateFrame("Frame", nil, UIParent)
tampa:SetPoint("BOTTOMLEFT", pixels[15], "TOPLEFT", 0, 0)
tampa:SetSize(5, altura_marcador)
local tex_tampa = tampa:CreateTexture(nil, "BACKGROUND")
tex_tampa:SetAllPoints()
tex_tampa:SetColorTexture(cor_marcador.r, cor_marcador.g, cor_marcador.b)
tampa.texture = tex_tampa
tampa:SetFrameStrata("TOOLTIP")

local x_text = UIParent:CreateFontString(nil, "OVERLAY", "GameFontNormal")
x_text:SetPoint("BOTTOMLEFT", UIParent, "BOTTOMLEFT", 3, 288)
x_text:SetText("X = 0.00")

local y_text = UIParent:CreateFontString(nil, "OVERLAY", "GameFontNormal")
y_text:SetPoint("TOPLEFT", x_text, "BOTTOMLEFT", 0, -2)
y_text:SetText("Y = 0.00")

-- atualiza os pixels
C_Timer.NewTicker(0.10, function()
    -- pixel 0 - HP / autoattack / target morto
    local hp_g = 0
    local hp_r = IsCurrentSpell("Attack") and 255 or 0
    local hp_b = UnitIsDead("target") and 255 or 0
    local hp_cur = UnitHealth("player")
    local hp_max = UnitHealthMax("player")
    if hp_max > 0 then
        hp_g = math.floor(hp_cur / hp_max * 255)
    end
    pixels[0].texture:SetColorTexture(hp_r/255, hp_g/255, hp_b/255)

-- pixel 1 - Mana (canal azul) + Slots livres (canal vermelho)
-- MANA
local mana_cur = UnitPower("player", 0)           -- mana atual
local mana_max = UnitPowerMax("player", 0)        -- mana máxima
local mana_b = (mana_max > 0) and math.floor(mana_cur / mana_max * 255) or 0  
-- SLOTS LIVRES NAS BAGS
local livres = 0
for bag = 0, 4 do
  local free, tipo = C_Container.GetContainerNumFreeSlots(bag)
  if tipo == 0 then livres = livres + free end
end
local bag_r = math.max(0, 255 - livres)           -- canal vermelho = 255 - slots livres (inverso)
-- APLICA NO PIXEL 1
pixels[1].texture:SetColorTexture(bag_r/255, 0, mana_b/255)
                                                  -- define cor do pixel 1: R = slots, B = mana


    -- pixel 2 - Posição X
    local r2, g2, b2 = 0, 0, 0
    local mapid = C_Map.GetBestMapForUnit("player")
    local pos = mapid and C_Map.GetPlayerMapPosition(mapid, "player")
    if pos then
        local x = pos.x * 100
        local intx = math.floor(x)
        local decx = (x - intx) * 100
        r2 = math.floor(intx / 10) * 25
        g2 = math.floor(decx * 2.5)
        b2 = (intx % 10) * 25
        x_text:SetText("X = " .. string.format("%.2f", x))
    else
        x_text:SetText("X = 0.00")
    end
    pixels[2].texture:SetColorTexture(r2/255, g2/255, b2/255)

    -- pixel 3 - Posição Y
    local r3, g3, b3 = 0, 0, 0
    if pos then
        local y = pos.y * 100
        local inty = math.floor(y)
        local decy = (y - inty) * 100
        r3 = math.floor(inty / 10) * 25
        g3 = math.floor(decy * 2.5)
        b3 = (inty % 10) * 25
        y_text:SetText("Y = " .. string.format("%.2f", y))
    else
        y_text:SetText("Y = 0.00")
    end
    pixels[3].texture:SetColorTexture(r3/255, g3/255, b3/255)

-- pixel 4 - Facing (R) + Speed invertido (G) + CD do Stoneform (B)
local r4 = math.floor(((GetPlayerFacing() or 0) / (2 * math.pi)) * 255) -- R: direção que o player está olhando (0–255)
local g4 = math.max(0, 255 - math.floor(GetUnitSpeed("player") or 0))  -- G: velocidade invertida (quanto menor a vel, maior o valor)
local b4 = 0                                                           -- B: cooldown do Stoneform em segundos (0–255)
local start, dur, enabled = GetSpellCooldown("Stoneform")              -- pega início, duração e se está ativada
if enabled == 1 and start > 0 and dur > 0 then                         -- se está em cooldown
    b4 = math.min(math.floor(dur - (GetTime() - start)), 255)         -- calcula o tempo restante, limitado a 255
end
pixels[4].texture:SetColorTexture(r4/255, g4/255, b4/255)             -- aplica os valores no pixel 4

-- pixel 5 - Combate + Debuffs
local r5 = UnitAffectingCombat("player") and 255 or 0             -- R: 255 se em combate
local g5 = (r5 == 0) and 255 or 0                                 -- G: 255 se fora de combate

-- B: codificação dos debuffs (Magic = 16, Disease = 8, Poison = 4, nil = 2, Curse = 1)
local debuff_val = 0
for i = 1, 40 do
    local _, _, _, dispelType = UnitDebuff("player", i)
    if not dispelType and _ then -- se tem debuff mas sem dispelType (ex: bleed)
        debuff_val = bit.bor(debuff_val, 2)
    elseif dispelType == "Magic" then
        debuff_val = bit.bor(debuff_val, 16)
    elseif dispelType == "Disease" then
        debuff_val = bit.bor(debuff_val, 8)
    elseif dispelType == "Poison" then
        debuff_val = bit.bor(debuff_val, 4)
    elseif dispelType == "Curse" then
        debuff_val = bit.bor(debuff_val, 1)
    end
end
local b5 = debuff_val * 8 -- Debuffs no player, igual vestibular (somatória)
pixels[5].texture:SetColorTexture(r5/255, g5/255, b5/255)


-- pixel 6 - Level (R) + Classe Paladino (G) + CD da Bolha (B)
local lvl = UnitLevel("player")
local _, class = UnitClass("player")
local b6 = 0 -- cooldown da bolha (Divine Protection)

local start, dur, enabled = GetSpellCooldown("Divine Protection")
if enabled == 1 and start > 0 and dur > 0 then
	b6 = math.min(math.floor(dur - (GetTime() - start)), 255)
end
pixels[6].texture:SetColorTexture(
	math.min(lvl * 4, 255)/255, -- canal vermelho = level × 4
	(class == "Paladin" and 255 or 0)/255, -- canal verde = 255 se paladino
	b6/255 -- canal azul = cooldown da bolha
)

    -- pixel 7 - Target HP (R), Existe (G)
    local r7, g7 = 0, 0
    if UnitExists("target") then
        local thp = UnitHealth("target")
        local thpmax = UnitHealthMax("target")
        if thpmax > 0 then
            r7 = math.floor(thp / thpmax * 255)
            g7 = 255
        end
    end
    pixels[7].texture:SetColorTexture(r7/255, g7/255, 0)

    -- pixel 8 - Target Level
    local r8 = (UnitExists("target") and math.min(UnitLevel("target") * 4, 255)) or 0
    pixels[8].texture:SetColorTexture(r8/255, 0, 0)

    -- pixel 9 - Judgement range (R), cooldown (G), melee (B)
    local r9, g9, b9 = 0, 0, 0
    if UnitExists("target") then
        if IsSpellInRange("Judgement", "target") == 1 then r9 = 255 end
        local s, d = GetSpellCooldown("Judgement")
        if s and d and d > 0 then
            g9 = math.min(255, math.floor((1 - (GetTime() - s)/d) * 255))
        end
        if CheckInteractDistance("target", 3) then b9 = 255 end
    end
    pixels[9].texture:SetColorTexture(r9/255, g9/255, b9/255)

    -- pixel 10 - Cast player
    local r10, g10, b10 = 0, 0, 0
    local nome, _, _, startTime, endTime = UnitCastingInfo("player")
    if nome then
        r10 = 255
        g10 = math.floor(((GetTime()*1000 - startTime) / (endTime - startTime)) * 255)
        b10 = string.byte(nome:sub(1,1)) or 0
    end
    pixels[10].texture:SetColorTexture(r10/255, g10/255, b10/255)

    -- pixel 11 - Cast target
    local r11, g11, b11 = 0, 0, 0
    nome, _, _, startTime, endTime = UnitCastingInfo("target")
    if nome then
        r11 = 255
        g11 = math.floor(((GetTime()*1000 - startTime) / (endTime - startTime)) * 255)
        b11 = string.byte(nome:sub(1,1)) or 0
    end
    pixels[11].texture:SetColorTexture(r11/255, g11/255, b11/255)

    -- pixel 12 - Buff SOR
    local r12, g12 = 0, 255
    for i = 1, 40 do
        local name, _, _, _, dur, expira = UnitBuff("player", i)
        if name and name:find("Seal of Righteousness") then
            r12 = 255
            if dur and expira then
                local t = expira - GetTime()
                g12 = math.max(0, 255 - math.floor(t * 4))
            end
            break
        end
    end
    pixels[12].texture:SetColorTexture(r12/255, g12/255, 0)

    -- pixel 13 - BoM, HoJ CD, HoJ em range
    local r13, g13, b13 = 0, 0, 0
    for i = 1, 40 do
        local name, _, _, _, dur, expira = UnitBuff("player", i)
        if name and name:find("Blessing of Might") then
            if dur and expira then
                local t = expira - GetTime()
                r13 = math.max(0, math.min(255, math.floor(t)))
            else
                r13 = 255
            end
            break
        end
    end
    local s, d = GetSpellCooldown("Hammer of Justice")
    if s and d and d > 0 then
        g13 = math.min(255, math.floor((1 - (GetTime() - s)/d) * 255))
    end
    if UnitExists("target") and IsSpellInRange("Hammer of Justice", "target") == 1 then
        b13 = 255
    end
    pixels[13].texture:SetColorTexture(r13/255, g13/255, b13/255)

    -- pixel 14 - Creature type
    local tipo = UnitCreatureType("target")
    local r14 = 0
    if tipo == "Humanoid" then r14 = 50
    elseif tipo == "Beast" then r14 = 100
    elseif tipo == "Undead" then r14 = 150
    elseif tipo == "Demon" then r14 = 200
    elseif tipo == "Elemental" then r14 = 210
    elseif tipo == "Mechanical" then r14 = 220
    elseif tipo == "Dragonkin" then r14 = 230
    elseif tipo == "Giant" then r14 = 240
    elseif tipo == "Critter" then r14 = 80 end
    pixels[14].texture:SetColorTexture(r14/255, 0, 0)

    -- pixel 15 - Reaction + Threat
    local r15, g15, b15 = 0, 0, 0
    if UnitExists("target") then
        local react = UnitReaction("player", "target")
        if react then
            if react <= 2 then r15 = 255
            elseif react <= 4 then r15, g15 = 255, 255
            elseif react >= 5 then g15 = 255 end
        end
        local threat = UnitThreatSituation("player", "target")
        if threat == 3 then b15 = 255 end
    end
    pixels[15].texture:SetColorTexture(r15/255, g15/255, b15/255)

end) -- ⬅️ esse é o fim do Ticker
