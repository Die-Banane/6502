        LDX #$02
        LDA #$05
        CLC

        ADC ($00,X)
        BRK
