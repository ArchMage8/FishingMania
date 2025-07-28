EXTERNAL EnablePlayerShop()
Hey, welcome back!
->main

=== main ===  
What's up?

    +[I got some fish I want to sell!]  
    ->section1

    +[Who’s that other worker?]  
    ->section2

    +[Nevermind.]  
    ->section3

=== section1 ===  
Sure! Lemme see what you've got!
~ EnablePlayerShop()
->END

=== section2 ===  
Oh, her?
She’s in charge of the market’s special deliveries!
You should talk to her about that!

    +[Is it different from selling things to you?]  
    ->section2A

    +[Sure, I'll do that!]  
    ->section2B

=== section2A ===  
You should ask her about that. Last time I tried getting involved, I messed up an order and got scolded...
->END

=== section2B ===  
Alrighty then!  
->END

=== section3 ===  
Ah, okay then...  
->END
