# Potprogram konverzija brojne vrednosti u oktalni string
# autor: Dimitrije Gasic SV 31-2021

.section .text
.globl octToStr

# unsigned int octToStr(char* ispis)

octToStr:
	pushl %ebp
	movl %esp, %ebp
	pushl %ebx
	pushl %esi
	pushl %edi

	movl 8(%ebp), %edi
	movl $8, %ebx      # baza = 8
cifra:
	xorl %edx, %edx
	divl %ebx
	addb $'0', %dl
	movb %dl, (%edi)
	incl %edi
	andl %eax, %eax    # kraj?
	jnz cifra
	movb $0, (%edi)    # ispis += '\0'
	decl %edi          # obrtanje
	movl 8(%ebp), %esi
obrni:
	cmpl %edi, %esi
	jae kraj
	movb (%esi), %ah
	movb (%edi), %al
	movb %al, (%esi)
	movb %ah, (%edi)
	decl %edi
	incl %esi
	jmp obrni
kraj:
	popl %edi
	popl %esi
	popl %ebx
	movl %ebp, %esp
	popl %ebp
	ret
