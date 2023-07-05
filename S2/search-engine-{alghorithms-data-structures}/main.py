from Utils.common import *
from App.client import Client

import os
from Utils.colors import *


def main():
    path = os.getcwd()
    # max duzina linije ne moze biti veca od sirine terminala

    cli = Client(path)

    title = 'HTML Search Engine'
    out_width = len(title)
    print(title)
    print(f"{'v 1.0.3' : ^{out_width}}")
    print(f"\n{'<help>  za pomoc' : ^{out_width}}")
    print(f"{'<q>   za izlaz' : ^{out_width}}\n")

    while True:
        cli_input = input(">> ").strip().split(' ')

        try:
            check_exit(cli_input)

            if cli_input[0] == 'cd':
                cli.cd(cli_input)

            elif cli_input[0] == 'ls':
                cli.ls(cli_input)

            elif cli_input[0] == 'pwd':
                cli.pwd(cli_input)
            
            elif cli_input[0] == 'cls':
                cli.cls(cli_input)

            elif cli_input[0] == 'search':
                cli.search(cli_input)
            
            elif cli_input[0] == 'settings':
                cli.settings(cli_input)

            elif cli_input[0] == 'help':
                if not cli.validate_command(cli_input, 1):
                    continue
                
                lines = [
                    "\n<cd> [args] Promeni direktorijum (.trenutni ..roditelj)",
                    "<ls>        Ispis fajlova trenutnog direktorijuma",
                    "<pwd>       Ispisi trenutnu putanju",
                    "<cls>       Ocisti terminal",
                    "<search>    Zapocni pretragu",
                    "<settings>  Podesavanja",
                    "<help>      Pomoc",
                    "<q>         Izlaz iz aplikacije\n",
                ]
                print('\n'.join(lines))
            
            else:
                print(warning("Neispravna komanda; <help> za pomoc"))
                
        except ExitException:
            break
        except ValueError as e:
                print(failure(e))
        except Exception:
            print(failure('Unresolved Exception:\n') + \
                    color_string(traceback.format_exc(), bcolors.CGREY))
            break


if __name__ == "__main__":
    main()
