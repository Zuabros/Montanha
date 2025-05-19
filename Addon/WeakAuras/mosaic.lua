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

-- pixel 1 - Mana (B) + Slots livres (R) + Erros de combate (G)

-- MANA
local mana_cur = UnitPower("player", 0)
local mana_max = UnitPowerMax("player", 0)
local mana_b = (mana_max > 0) and math.floor(mana_cur / mana_max * 255) or 0

-- SLOTS LIVRES NAS BAGS
local livres = 0
for bag = 0, 4 do
  local free, tipo = C_Container.GetContainerNumFreeSlots(bag)
  if tipo == 0 then livres = livres + free end
end
local bag_r = math.max(0, 255 - livres)

-- ERROS DE COMBATE (canal G)
if erroG == nil then erroG = 0 end
if erro_timeout == nil then erro_timeout = 0 end

-- Frame de erro com detecção por mensagem
if not erro_frame then
  erro_frame = CreateFrame("Frame")
  erro_frame:RegisterEvent("UI_ERROR_MESSAGE")
  erro_frame:SetScript("OnEvent", function(self, event, errType, msg)
    local novoErro = 0

if msg and (msg:lower():find("facing") or msg:lower():find("Herbalism")) then
      novoErro = novoErro + 128 -- erro de facing
    end
    if msg == "Out of range." or msg == "You are too far away!" then
      novoErro = novoErro + 64 -- erro de range
    end

    if novoErro > 0 then
      erroG = novoErro
      erro_timeout = 10 -- mantém por 10 ticks de 0.10s = 1 segundo
    end
  end)
end

-- Ticker de 0.10s para expiração do erro
if not erro_ticker then
  erro_ticker = C_Timer.NewTicker(0.10, function()
    if erro_timeout > 0 then
      erro_timeout = erro_timeout - 1
      if erro_timeout == 0 then
        erroG = 0
      end
    end
  end)
end

-- APLICA NO PIXEL 1: R = bag, G = erro, B = mana
pixels[1].texture:SetColorTexture(bag_r/255, erroG/255, mana_b/255)


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

-- -------------------------------------
-- pixel 5 - Combate + Nadando + Debuffs
-- -------------------------------------

-- R: 255 se em combate, 0 se fora
local r5 = UnitAffectingCombat("player") and 255 or 0

-- G: 255 se nadando, 0 se não
local g5 = IsSwimming() and 255 or 0

-- B: codificação dos debuffs (Magic = 16, Disease = 8, Poison = 4, nil = 2, Curse = 1)
local debuff_val = 0
for i = 1, 40 do
    local _, _, _, dispelType = UnitDebuff("player", i)
    if not dispelType and _ then
        debuff_val = bit.bor(debuff_val, 2) -- sem tipo (ex: bleed)
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

local b5 = debuff_val * 8 -- amplificado pra granular o azul

pixels[5].texture:SetColorTexture(r5/255, g5/255, b5/255)
-- -------------------------------------
-- PIXEL 6 - Level (R) + Classe (G) + FLAGS (B)
-- -------------------------------------
local lvl = UnitLevel("player")
local _, class = UnitClass("player")

-- codificação da própria classe (sem targets)
local class_val = 0
if class == "Paladin" then class_val = 255
elseif class == "Warrior" then class_val = 250
elseif class == "Hunter" then class_val = 245
elseif class == "Rogue" then class_val = 240
elseif class == "Priest" then class_val = 235
elseif class == "Shaman" then class_val = 230
elseif class == "Mage" then class_val = 225
elseif class == "Warlock" then class_val = 220
elseif class == "Druid" then class_val = 215
end

-- codificação das flags no canal B
local b6 = 0

-- bolha (Divine Protection) disponível?
local dpName = GetSpellInfo("Divine Protection")
if dpName then
    local s, d, e = GetSpellCooldown(dpName)
    if e == 1 and (s == 0 or d == 0) then
        b6 = b6 + 128
    end
end

-- exorcism disponível?
local exoName = GetSpellInfo("Exorcism")
if exoName then
    local s2, d2, e2 = GetSpellCooldown(exoName)
    if e2 == 1 and (s2 == 0 or d2 == 0) then
        b6 = b6 + 64
    end
end

-- target com debuff contendo "justice"
for i = 1, 40 do
    local name = UnitDebuff("target", i)
    if name and name:lower():find("justice") then
        b6 = b6 + 32
        break
    end
end

-- exorcism está em alcance?
if exoName and UnitExists("target") and IsSpellInRange(exoName, "target") == 1 then
    b6 = b6 + 16
end

-- aplica no pixel
pixels[6].texture:SetColorTexture(
    math.min(lvl * 4, 255) / 255,  -- R: nível × 4
    class_val / 255,               -- G: valor da classe
    b6 / 255                       -- B: flags codificadas
)

-- pixel 7 - Target HP (R), Existe (G), Level (B)
local r7, g7, b7 = 0, 0, 0
if UnitExists("target") then
    local thp = UnitHealth("target")
    local thpmax = UnitHealthMax("target")
    if thpmax > 0 then
        r7 = math.floor(thp / thpmax * 255)       -- HP em percentual (0 a 255)
        g7 = 255                                   -- Confirma existência
        b7 = UnitLevel("target") * 4               -- Level × 4 (cap implícito em 63)
    end
end

pixels[7].texture:SetColorTexture(r7/255, g7/255, b7/255)

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

-- pixel 12 - Buff SOR + CD do Lay on Hands
local r12, g12, b12 = 0, 255, 0

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

-- cooldown do Lay on Hands
local s, d, e = GetSpellCooldown("Lay on Hands")
if e == 1 and s > 0 and d > 0 then
    b12 = math.min(math.floor(d - (GetTime() - s)), 255)
end

pixels[12].texture:SetColorTexture(r12/255, g12/255, b12/255)

-- pixel 13 - Blessings ativos (R) + CD do HoJ (G) + alcance do HoJ (B)
local r13, g13, b13 = 0, 0, 0
local bless_val = 0
for i = 1, 40 do
    local name = UnitBuff("player", i)
    if name then
        if name:find("Blessing of Might")      then bless_val = bit.bor(bless_val, 1) end
        if name:find("Blessing of Wisdom")     then bless_val = bit.bor(bless_val, 2) end
        if name:find("Blessing of Kings")      then bless_val = bit.bor(bless_val, 4) end
        if name:find("Blessing of Salvation")  then bless_val = bit.bor(bless_val, 8) end
        if name:find("Blessing of Sanctuary")  then bless_val = bit.bor(bless_val, 16) end
        if name:find("Blessing of Freedom")    then bless_val = bit.bor(bless_val, 32) end
        if name:find("Blessing of Protection") then bless_val = bit.bor(bless_val, 64) end
    end
end
r13 = bit.bor(bless_val, 128) -- força o bit 7 pra manter o valor alto e visível
-- canal G = cooldown do Hammer of Justice (inverso normalizado)
local s, d = GetSpellCooldown("Hammer of Justice")
if s and d and d > 0 then
    g13 = math.min(255, math.floor((1 - (GetTime() - s)/d) * 255))
end
-- canal B = HoJ em alcance
if UnitExists("target") and IsSpellInRange("Hammer of Justice", "target") == 1 then
    b13 = 255
end
pixels[13].texture:SetColorTexture(r13/255, g13/255, b13/255)

-- -------------------------------------
-- PIXEL 14 - TIPO DA CRIATURA DO TARGET
-- -------------------------------------
local r14 = 0

if UnitExists("target") then
    if UnitIsPlayer("target") and UnitIsFriend("player", "target") then
        local _, class = UnitClass("target")
        if class == "MAGE" or class == "WARLOCK" or class == "PRIEST" or class == "SHAMAN" then
            r14 = 110 -- caster (Blessing of Wisdom)
        else
            r14 = 105 -- melee (Blessing of Might)
        end
    else
        local creatureType = UnitCreatureType("target")
        local creatureTypes = {
            ["Humanoid"] = 50,
            ["Beast"] = 100,
            ["Undead"] = 150,
            ["Demon"] = 200,
            ["Elemental"] = 210,
            ["Mechanical"] = 220,
            ["Dragonkin"] = 230,
            ["Giant"] = 240,
            ["Critter"] = 80
        }
        r14 = creatureTypes[creatureType] or 0
    end


end

pixels[14].texture:SetColorTexture(r14 / 255, 0, 0)




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