INCLUDE globals.ink
EXTERNAL PickUpItem()

{itemcubo1 == true: ->full | ->Empty}



=== full ===
Habia algo en la papelera#speaker:narrador#audio:beep_1

->EJEMPLO(false)



=== EJEMPLO(chosen) ===
~ itemcubo1 = chosen
~ PickUpItem()
-> END

=== Empty ===
Esta vacia
->END
