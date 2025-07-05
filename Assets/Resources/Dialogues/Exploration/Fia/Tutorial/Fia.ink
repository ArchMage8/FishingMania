Hi! #speaker: Fia #trigger: next
Welcome to Fia's Restaurant! #speaker: Fia
I've never seen you around before... #speaker: Fia
Hey you looking for a job?

    +[Job?]
    ->Section1
    


=== Section1 ===
Yes a job?
Wait you can cook right?
    +[Cook?]
    ->Section2
    
    +[Weird question to ask someone...]
    ->Section3
    
=== Section2 ===
Yes cook!
    +[I guess so...]
    ->Section4

=== Section3 ===
Never mind that...
Can you cook?
    +[I guess so...]
    ->Section4
    
=== Section4 ===
Great I'll be open from 5am to 10pm
You can sell me the dishes you made!
Or even the fishes you catch!
See you soon!
->END
