# Potprogram konverzija heksadecimalnog string-a u brojnu vrednost
# (ignorise velicinu slova)
# autor: Dimitrije Gasic SV 31-2021

.section .text
.globl strToHex

# unsigned int strToHex(char* string, unsigned int* greska)

strToHex:
	pushl %ebp
	movl %esp, %ebp
	subl $4, %esp
	pushl %ebx
	pushl %esi

	movl 8(%ebp), %esi
	movl $0, -4(%ebp)  # greska = 0
	xorl %eax, %eax    # r = 0
	xorl %ebx, %ebx
	movl $16, %ecx     # baza = 16
cifra:
	movb (%esi), %bl
	andb %bl, %bl
	jz kraj_br
	cmpb $'9', %bl
	jle broj
	cmpb $'F', %bl
	jle veliko
malo:
	subb $87, %bl     # a -> 10
	jmp dodaj
veliko:
	subb $55, %bl     # A -> 10
	jmp dodaj
broj:
	subb $'0', %bl    # cifra
dodaj:
	cmpb $15, %bl     # 0 <= cifra <= 15
	jg greska
	cmpb $0, %bl
	jl greska

	mull %ecx         # r *= baza
	addl %ebx, %eax   # r = r + cifra
	incl %esi
	jmp cifra
kraj_br:
	cmpl 8(%ebp), %esi  # prazan?
	je greska
	cmpl $255, %eax
	jle kraj
greska:
	incl -4(%ebp)       # ++greska
kraj:
	movl -4(%ebp), %ebx
	movl 12(%ebp), %edx
	movl %ebx, (%edx)   # upisivanje greske

	popl %esi
	popl %ebx
	movl %ebp, %esp
	popl %ebp
	ret
