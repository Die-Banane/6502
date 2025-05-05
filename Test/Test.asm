; LDA Immediate
LDA #$42         ; A = $42
STA $0400        ; $0400 = $42

; LDX Immediate
LDX #$05         ; X = $05
STX $0401        ; $0401 = $05

; LDY Immediate
LDY #$10         ; Y = $10
STY $0402        ; $0402 = $10

; TAX / TAY
TAX              ; A → X (X = $42)
TAY              ; A → Y (Y = $42)
STX $0403        ; $0403 = $42
STY $0404        ; $0404 = $42

; INX, INY, DEX, DEY
INX              ; X = $43
INY              ; Y = $43
DEX              ; X = $42
DEY              ; Y = $42
STX $0405        ; $0405 = $42
STY $0406        ; $0406 = $42

; ADC (Add with Carry)
CLC              ; Clear Carry
ADC #$10         ; A = $42 + $10 = $52
STA $0407        ; $0407 = $52

; SBC (Subtract with Carry)
SEC              ; Set Carry (nötig für SBC!)
SBC #$02         ; A = $52 - $02 = $50
STA $0408        ; $0408 = $50

; Bitwise operations
AND #$F0         ; A = $50 & $F0 = $50
STA $0409        ; $0409 = $50

ORA #$0F         ; A = $50 | $0F = $5F
STA $040A        ; $040A = $5F

EOR #$FF         ; A = $5F ^ $FF = $A0
STA $040B        ; $040B = $A0

; BRK – beendet Programm
BRK
