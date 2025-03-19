What are you looking at me for? #speaker: Magu #trigger: next

Lets go outside and catch something to eat… #speaker: Magu #trigger: next
    +[Eat?]
    ->Section1
    
    +[How do we do that?]
    ->Section2
    
=== Section1 ===
Yes, You just ruined my dinner after all! #speaker: Magu #trigger: branch1
->END

=== Section2 ===
Lets just go outside! #speaker: Magu #trigger: branch2
Exit is right there. #speaker: Magu #trigger: next
->END