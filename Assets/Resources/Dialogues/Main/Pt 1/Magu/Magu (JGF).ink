Hmm...? #speaker: Magu #trigger: next
->main

=== main ===
Whats do you want? #speaker: Magu #trigger: next
    +[What is this place]
    ->Section1
    
    +[Who are you?]
    ->Section2

    +[See ya!]
    ->Section3

=== Section1 ===
You don't know where you are? #speaker: Magu #trigger: branch1

I washed up here remember? #speaker: Iris #trigger: next

Ah yes! #speaker: Magu #trigger: next

This is Himawari Island! #speaker: Magu #trigger: next
At least that's what the humans call it! #speaker: Magu #trigger: next

->END

=== Section2 ===
I'm Magu!#speaker: Magu #trigger: branch2
And I own this house! #speaker: Magu #trigger: next

A cat owns this house? #speaker: Iris #trigger: next

...#speaker: Magu #trigger: next
Well the old owner left and sort of never came back... #speaker: Magu #trigger: next

What happned to him? #speaker: Iris #trigger: next

No idea! #speaker: Magu #trigger: next

Hmm... #speaker: Iris #trigger: next
->END

=== Section3 ===
... #speaker: Magu #trigger: branch3
->END