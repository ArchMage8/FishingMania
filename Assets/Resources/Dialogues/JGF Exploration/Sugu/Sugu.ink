Hello there #speaker: Sugu #trigger: next
->main

=== main ===
Another "Tourist" i see? #speaker: Sugu #trigger: next
    +[Who are you?]
    ->Section1
    
    +[What is this building?]
    ->Section2
    
    +[See ya]
    ->Section3


=== Section1 ===
I should ask you that... #speaker: Sugu

But nevermind... Ahem! #speaker: Sugu
I am Sugu! Owner of the largest shop in town! #speaker: Sugu

We sell a bunch of convenient items for your needs. #speaker: Sugu

    +[Can I buy something?]
    ->Section1A

=== Section1A ===
Unfortunately we're waiting for the courier.. sorry for the inconvenience. #speaker: Sugu
->END

=== Section2 ===
It's a general store! #speaker: Sugu
People come here everyday to shop for daily goods. #speaker: Sugu
But we are currently closed during this hour. #speaker: Sugu

->END
=== Section3 ===
Come back next time!
->END