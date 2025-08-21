EXTERNAL EnablePlayerShop()
Hey, welcome back! #speaker: Taisuke #trigger: next
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
Sure! Lets see what you've got!
~ EnablePlayerShop()
->END

=== section2 ===  
Oh, him?
He’s in charge of the market’s special deliveries!
You should talk to him about that!

    +[Is it different from selling things to you?]
    ->section2A

    +[Sure, I'll do that!]
    ->section2B

=== section2A ===  
You should ask her about that. I've got enough on my plate to deal with...
->END

=== section2B ===  
Alright then!  
->END

=== section3 ===  
Okay then...  
->END
