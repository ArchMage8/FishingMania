EXTERNAL EnablePlayerShop()

Hello! #speaker: Fia #trigger: next
Welcome to my restaurant!
Looking for a job?
->main

=== main ===
    +[Job?]
    ->Section1
    +[How much would you take for these?]
    ->Section2
    +[See ya!]
    ->Section3

=== Section1 ===
Yes job!
See my cook just quit, so I'm handling everything myself...
Getting the fish! Cooking the fish! Waiting tables!
    +[What am i supposed to do?]
    ->Section4

=== Section2 ===
Oohh, lemme see!
~ EnablePlayerShop()
->END

=== Section3 ===
Ciao!
->END

=== Section4 ===
Well I need a person to handle the fish and cooking problems...
So if you got any of those, just drop them off here
I'll take them off your hands!
    +[What exactly can I sell to you?]
    ->Section5

=== Section5 ===
....
The fish you catch, assuming you are a good fisherman...
and dishes you cook up, assuming they are good dishes...
->Section6

=== Section6===
So...?
    +[How much for these?]
    ->Section2
    +[I don't have anything right now]
    ->Section3