
#include <cstdlib>
#include <iostream>
int main() {
system("lspci -vmm | grep -E \"(Vendor|Device)\"");
return 0;
}
