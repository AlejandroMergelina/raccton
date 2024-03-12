INCLUDE globals.ink
EXTERNAL PickUpItem()

{itemcubo1 == "": ->full | ->Empty}



=== full ===
Habia algo en la papelera#speaker:narrador#audio:beep_1

->EJEMPLO("mal")



=== EJEMPLO(chosen) ===
~ itemcubo1 = chosen
~ PickUpItem()
-> END

=== Empty ===
Esta vacia
->END
