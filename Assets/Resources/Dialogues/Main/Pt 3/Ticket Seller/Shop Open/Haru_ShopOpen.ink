EXTERNAL EnableShop()

Hey, Your back! #speaker: Haru #trigger: next
Did ya need something?
->main

=== main ===
    +[So I can buy a ticket from you?]
    ->OpenShop
    
    +[So how are things?]
    ->Idle
    
    +[Nevermind...]
    ->Exit



=== OpenShop ===
Yep! Here take a look!
~ EnableShop()
->END


=== Idle ===
Oh, you know...
Not much, just the usual smells coming from the daily fish deliveries
->END

=== Exit ===
See ya!
->END
