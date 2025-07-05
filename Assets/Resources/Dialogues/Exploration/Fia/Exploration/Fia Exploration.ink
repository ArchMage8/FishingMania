EXTERNAL EnablePlayerShop()

Hello! #speaker: Fia #trigger: next
Welcome back to my restaurant!
Got anything to sell to me?
->main

=== main ===
    +[What can I sell to you again]
    ->Section1
    +[How much would you take for these?]
    ->Section2
    +[Not at the moment]
    ->Section3

=== Section1 ===
The fish you catch, assuming you are a good fisherman...
and dishes you cook up, assuming they are good dishes...
->Section6

=== Section2 ===
Oohh, lemme see!
~ EnablePlayerShop()
->END

=== Section3 ===
Ciao!
->END

=== Section6===
So...?
    +[How much for these?]
    ->Section2
    +[I don't have anything right now]
    ->Section3