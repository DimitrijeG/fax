from menus import *


def main():
    resolve_reservation_statuses()
    recalculate_apartments_availability()
    
    user = None
    running = True
    
    while running:
        menu = general_menu() + blank_line()
        if not user:
            menu += unsigned_menu()
        else:
            if user['role'] == 'guest':
                menu += guest_menu()
            elif user['role'] == 'host':
                menu += host_menu()
            elif user['role'] == 'admin':
                menu += admin_menu()
            menu += signed_menu()
    
        menu = activate_menu(menu)

        while True:
            choice = input("\nIzaberite opciju: ")
            if choice == 'x':
                running = False
                break
            elif choice not in menu.keys():
                print("\nIzabrali ste nepostojeću opciju")
            else:
                user = menu[choice](user)
                clear_console()
                break


if __name__ == "__main__":
    main()
