INCLUDE globals.ink
EXTERNAL PickUpItem(Item, quantity)

{itemcubo1 == true: ->full | ->Empty}



=== full ===
Habia algo en la papelera#speaker:narrador#audio:beep_1

->EJEMPLO(false)



=== EJEMPLO(chosen) ===
~ itemcubo1 = chosen

~ PickUpItem("Pocion de vida", 1)
-> END

=== Empty ===
Esta vacia
->END
