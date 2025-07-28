Hello there! #speaker: Sugu #trigger: next
->main

=== main ===
Another "Tourist" I see? #speaker: Sugu #trigger: next
    +[Who are you?]
    ->Section1



=== Section1 ===
I should ask you that... #speaker: Sugu

But nevermind, ahem! #speaker: Sugu
I am Sugu! Owner of the town's general store! #speaker: Sugu
We sell a bunch of goods for your needs. #speaker: Sugu

    +[What do you sell specifically?]
    ->Section1A

=== Section1A ===
Do you really want me to list everything I sell here?
    +[Uh, not all...]
    ->Section2

=== Section2 ===
Ugh, nevermind.
We sell goods you normally need for cooking.
Like salt, soy sauce, etc.

    +[Ah, ok.]
    ->Section3
    
=== Section3 ===
I'm open from 5am to 7pm.
Hope to see you soon.
->END