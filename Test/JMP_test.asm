        LDX #$01
        JMP direct

indirectTarget:
        LDA #$42
        BRK

indirectAddr:
        .word indirectTarget

direct:
        LDA #$69
        JMP (indirectAddr)
