EXTERNAL EnableShop()

Welcome back to my store! #speaker: Sugu #trigger: next
How can I help you?  #speaker: Sugu
->main

=== main ===
    +[Do you sell everything here?]
        -> section1
    +[I want to buy some stuff!]
        -> section2
    +[See ya!]
        -> section3

=== section1 ===
Well, no.
There are several stores here and across the other islands.
Each specializing in something.
-> section4

=== section2 ===
Sure! Take a look!  #speaker: Sugu
~ EnableShop()
->END

=== section3 ===
Come again!  #speaker: Sugu
->END

=== section4 ===
  +[I want to buy some stuff.]
        -> section2
    +[See ya!]
        -> section3
