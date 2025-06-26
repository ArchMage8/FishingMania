EXTERNAL EnableShop()

Welcome to my store! #speaker: Sugu #trigger: next
How can i help you?  #speaker: Sugu
->main

=== main ===
    +[What is this place?]
        -> section1
    +[I want to buy some stuff]
        -> section2
    +[See ya!]
        -> section3

=== section1 ===
This is the general store.  #speaker: Sugu
I sell everything you need here to make dishes.  #speaker: Sugu
Well... Everything you can't fish out of the sea.  #speaker: Sugu
-> main

=== section2 ===
Sure take a look...  #speaker: Sugu
~ EnableShop()
->END

=== section3 ===
Come again!  #speaker: Sugu
->END
