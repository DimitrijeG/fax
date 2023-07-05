# Potprogram konverzija decimalnog string-a u brojnu vrednost
# autor: Dimitrije Gasic SV 31-2021

.section .text
.globl strToDec

# unsigned int strToDec(char* string, unsigned int* greska)

strToDec:
	pushl %ebp
	movl %esp, %ebp
	subl $4, %esp
	pushl %ebx
	pushl %esi

	movl 8(%ebp), %esi
	movl $0, -4(%ebp)  # greska = 0
	xorl %eax, %eax    # r = 0
	xorl %ebx, %ebx
	movl $10, %ecx     # baza = 10
cifra:
	movb (%esi), %bl
	andb %bl, %bl
	jz kraj_br

	subb $'0', %bl    # cifra
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
