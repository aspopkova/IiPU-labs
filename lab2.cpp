#include <cstdlib>
#include <iostream>

int main() {
system("sudo hdparm -I /dev/sda | grep Number");
system("sudo hdparm -I /dev/sda | grep Firmware");
system("sudo hdparm -I /dev/sda | grep PIO");
system("sudo hdparm -I /dev/sda | grep DMA");
system("sudo hdparm -I /dev/sda | grep Supported");
system("df -h");
return 0;
}