Hi! #speaker: Fia #trigger: next
Welcome to Fia's Restaurant! #speaker: Fia
I've never seen you around before... #speaker: Fia

    +[Restaurant?]
    ->Section1
    +[Yes I just "arrived"]
    ->Section2
    +[See ya]
    ->Section3
    


=== Section1 ===
Yes, It is Fia's place, serving from 5am! #speaker: Fia
->END

=== Section2 ===
Ah! Can you cook? #speaker: Fia
    +[Yes?]
    ->Section2A

=== Section2A
I'm looking for a new cook! #speaker: Fia
Comeback later when the place is open! #speaker: Fia
->END

=== Section3 ===
Bye Bye! #speaker: Fia
->END
