#!/bin/bash

# Write counters to provision_logs.txt
log_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/provision_logs.txt"
{
  echo "================================================="
  printf "%-30s : %s\n" "Title" "$1"
  printf "%-30s : %d\n" "Total records processed" "$2"
  printf "%-30s : %d\n" "Records already exist" "$3"
  printf "%-30s : %d\n" "Errors encountered" "$4"
  echo "================================================="
  # Print ASCII art only if $4 > 0
  if [ "$4" -gt 0 ]; then
    echo 
    echo "███████╗██████╗ ██████╗  ██████╗ ██████╗         ██████╗  ██████╗ ██████╗██╗   ██╗██████╗ ███████╗██████╗ "
    echo "██╔════╝██╔══██╗██╔══██╗██╔═══██╗██╔══██╗       ██╔═══██╗██╔════╝██╔════╝██║   ██║██╔══██╗██╔════╝██╔══██╗"
    echo "█████╗  ██████╔╝██████╔╝██║   ██║██████╔╝       ██║   ██║██║     ██║     ██║   ██║██████╔╝█████╗  ██║  ██║"
    echo "██╔══╝  ██╔══██╗██╔══██╗██║   ██║██╔══██╗       ██║   ██║██║     ██║     ██║   ██║██╔══██╗██╔══╝  ██║  ██║"
    echo "███████╗██║  ██║██║  ██║╚██████╔╝██║  ██║       ╚██████╔╝╚██████╗╚██████╗╚██████╔╝██║  ██║███████╗██████╔╝"
    echo "╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝        ╚═════╝  ╚═════╝ ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═════╝ " 
    echo  
  fi
  echo
} >> "$log_file"