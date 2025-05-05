; Start (du kannst ihn z. B. bei 0x0200 oder 0x0600 im RAM ablegen)

        LDX #$00         ; X = 0
        LDY #$10         ; Y = 16
        STX $0100        ; Speicher[0x0100] = 0
        STY $0101        ; Speicher[0x0101] = 16

        LDA #$05         ; A = 5
        STA $0102        ; Speicher[0x0102] = 5

        LDA $0101        ; A = 16
        ADC #$0A         ; A += 10 → A = 26 (0x1A)
        STA $0103        ; Speicher[0x0103] = 26

        SBC #$05         ; A -= 5 → A = 21 (0x15)
        STA $0104        ; Speicher[0x0104] = 21

        CMP #$15         ; Vergleich mit 21 → Zero-Flag wird gesetzt
        BEQ equal_label  ; Sollte springen (da A == $15)

        LDA #$00         ; Wird übersprungen
        STA $010F

equal_label:
        LDA #$55
        STA $0105

        PHA              ; Push 0x55 auf Stack
        LDA #$00         ; A = 0
        PLA              ; A = 0x55
        STA $0106

        JSR subroutine   ; Springt zu Subroutine und kommt zurück

        INC $0100        ; 0 → 1
        DEC $0101        ; 16 → 15

        ASL $0102        ; 5 << 1 = 10
        ROR $0103        ; 26 >> 1 mit Carry = 13

        BRK              ; Programmende

; ----------- Subroutine -------------
subroutine:
        LDA #$42
        STA $0107
        RTS
