# SOV 2020
# autor: Dimitrije Gasic SV 31-2021

.section .data
prompt1: .ascii "Unseite logicki izraz: \0"   # lapsus calami nije promenjeno zbog testova
LEN1 = . - prompt1
prompt2: .ascii "Rezultat: \0"
LEN2 = . - prompt2
err1: .ascii "Greska kod operanda.\n\0"
ERR1_LEN = . - err1
err2: .ascii "Greska kod operacije.\n\0"
ERR2_LEN = . - err2
newline: .ascii "\n\0"

MAX_LEN = 100
unos: .fill MAX_LEN,1,0

temp_rec: .fill 15,1,0  # pomocna varijabla

operand1: .byte 0       # ovde ce se na kraju nalaziti resenje
operand2: .byte 0
operacija: .byte 0

MAX_ISPIS = 9
ispis: .fill MAX_ISPIS,1,0
ispis_len: .long 0

error: .long 0
.section .text
.globl main

main:
						 # ispis za unos izraza
	movl $4, %eax
	movl $1, %ebx
	leal prompt1, %ecx
	movl $LEN1, %edx
	int $0x80

						 # unos izraza
	movl $3, %eax
	movl $0, %ebx
	leal unos, %ecx
	movl $MAX_LEN, %edx
	int $0x80

	movl $unos, %esi
	movl $temp_rec, %edi
	movl $1, %ebx          # flag: 1=operand1 2=operacija 3=operand2
parsiranje:
	movb (%esi), %al       # posmatrani karakter
	cmpb $10, %al
	je kraj_reci
	cmpb $32, %al
	je kraj_reci
	movb %al, (%edi)       # temp_rec += karakter
	incl %edi
	jmp sledeci_karakter
kraj_reci:
	movb $0, (%edi)        # temp_rec += '\0'

	movl $temp_rec, %edi   # prvi karakter reci pomocu kog se
	cmpb $'x', 1(%edi)     #  	odredjuje sta ona predstavlja
	je hex
	cmpb $'X', 1(%edi)
	je hex
	cmpb $'9', (%edi)
	jbe dec

	testl $1, %ebx
	jnz greska_unos        # operacija nije na redu
	cmpb $0, 1(%edi)
	jne greska_unos        # operacija ima vise od 1 karaktera
	jmp upisi_operaciju
hex:
	cmpb $'0', (%edi)
	jne greska_unos

	addl $2, %edi          # adresa prve cifre (posle '0x')
	pushl $error
	pushl %edi
	call strToHex
	addl $8, %esp
	jmp upisi_broj
dec:
	cmpb $'0', (%edi)
	jb greska_unos

	pushl $error
	pushl %edi
	call strToDec
	addl $8, %esp

upisi_broj:
	cmpl $1, error
	je greska_unos         # konverzija nije bila uspesna

	cmpl $1, %ebx
	je prvi_operand
	cmpl $3, %ebx
	je drugi_operand
	jmp greska_unos        # operacija nije na redu

	prvi_operand:
	movb %al, operand1
	incl %ebx              # flag=2
	jmp sledeca_rec
	drugi_operand:
	movb %al, operand2
	movb operand1, %cl

	cmpb $'^', operacija
	je eili
	xchg %eax, %ecx        # zato sto samo ecx moze da vrsi iteraciju u rol
	cmpb $'>', operacija
	je rot_desno
	cmpb $'<', operacija
	jne error2             # neispravna operacija
	rot_levo:
		rolb %cl, %al      # operand1 < operand2
		jmp pomeri
	rot_desno:
		rorb %cl, %al      # operand1 > operand2
		jmp pomeri
	eili:
		xorb %cl, %al      # operand1 ^ operand2
	pomeri:
	movb %al, operand1
	decl %ebx              # flag=2
	jmp sledeca_rec

upisi_operaciju:
	movb (%edi), %al
	movb %al, operacija
	incl %ebx              # flag=3

sledeca_rec:
	movl $temp_rec, %edi
sledeci_karakter:
	cmpb $10, (%esi)       # kraj string-a?
	je ispis_resenja
	incl %esi
	jmp parsiranje

greska_unos:
	testl $1, %ebx
	jnz error1             # ako %ebx == 1 ili 3 red je na operand
	jz error2              # ako %ebx == 2 tada je red na operaciju

ispis_resenja:
	xorl %eax, %eax
	movb operand1, %al     # previse sam glup da prosledim byte u potprogram

	pushl $ispis
	call octToStr
	addl $8, %esp

	movl $4, %eax
	movl $1, %ebx
	leal prompt2, %ecx
	movl $LEN2, %edx
	int $0x80
						# ispis resenja
	movl $4, %eax
	movl $1, %ebx
	leal ispis, %ecx
	movl $MAX_ISPIS, %edx
	int $0x80

	movl $0, %ebx       # izlazni kod 0
	jmp kraj
error1:
	movl $4, %eax
	movl $1, %ebx
	leal err1, %ecx
	movl $ERR1_LEN, %edx
	int $0x80
	movl $1, %ebx       # izlazni kod 1
	jmp kraj
error2:
	movl $4, %eax
	movl $1, %ebx
	leal err2, %ecx
	movl $ERR2_LEN, %edx
	int $0x80
	movl $1, %ebx       # izlazni kod 1
kraj:
	movl $1, %eax
	int $0x80
