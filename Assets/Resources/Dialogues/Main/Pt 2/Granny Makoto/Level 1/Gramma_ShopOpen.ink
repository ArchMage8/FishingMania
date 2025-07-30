EXTERNAL EnableShop()


Welcome back dear...  #speaker: Granny Makoto #trigger: next
->main

=== main ===

    +[Here for some ingredients granny!]
    ->section2
    
    +[How are things today granny?]
    ->section3
    
    +[Nevermind, sorry for bothering you granny...]
    ->section4

=== section2 ===
Sure! Take a look dear... #speaker: Granny Makoto
~ EnableShop()
->END

=== section3 ===
Oh, you know, the usual here and there... #speaker: Granny Makoto
Stocking shelves, helping people decide what to make for dinner... #speaker: Granny Makoto

    +[So nothing really happened?]
    ->section3A
    
    +[Ah, ok then...]
    ->section3B

=== section3A ===
Well, that cranky Kyuri came by the shop... #speaker: Granny Makoto
Complaining about something again... #speaker: Granny Makoto
I've learned to "nod and smile" if he goes on a rant... #speaker: Granny Makoto
->END

=== section3B ===
Well, helping people with their cooking is a lot of talking... #speaker: Granny Makoto
I'm surprised by the number of people who come to my store not knowing what to buy... #speaker: Granny Makoto
->END

=== section4 ===
No problem dear... #speaker: Granny Makoto
->END