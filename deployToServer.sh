#!/usr/bin/env bash
docker save -o ./backend.tar customerdrivencd_customerdriven-backend
docker save -o ./frontend.tar customerdrivencd_customerdriven-frontend
scp ./backend.tar FishFarmAdmin@137.116.234.188:/home/FishFarmAdmin/build
scp ./frontend.tar FishFarmAdmin@137.116.234.188:/home/FishFarmAdmin/build
rm backend.tar
rm frontend.tar
ssh FishFarmAdmin@137.116.234.188 'sudo ./build/build.sh'
