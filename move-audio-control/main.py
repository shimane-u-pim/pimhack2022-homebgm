#!/usr/bin/env python
import time
import numpy as np
import cv2
import requests

###############################################################################
# Configurations
###############################################################################

VIDEO_DEVICE_ID = 0

# 動態検知対象のフレーム数
MOVE_EVAL_FRAME = 3

# 動態検知において変動があったピクセル数のしきい値
# これは、MOVE_EVAL_FRAMEフレームの平均を演算します。
MOVE_EVAL_MEAN = 6000

# フレームの保持数
MAX_HOLD_FRAMES = 10

# 減音に入るまでの無動作時間
DECIDE_LOWER_TIME = 20

# ミュートに入るまでの無動作時間
DECIDE_MUTE_TIME = 30

# 標準時音量
DEFAULT_VOLUME = 0.125

# 原音時音量
LOWER_VOLUME = 0.075

AUDIO_DEVICE_ID = 'Main'

AUDIO_PLAYER_ENDPOINT = 'http://127.0.0.1:8214/'

###############################################################################
# Program
###############################################################################

cap = cv2.VideoCapture(VIDEO_DEVICE_ID)

last = None

last_th = []

last_move_time = time.time()

mode_holder = 'normal'

sysmode = 'normal'


def volreq(vol):
    headers = {'content-type': 'application/json, charset=utf-8'}
    response = requests.post(
        AUDIO_PLAYER_ENDPOINT, json={'type': 'vol',
                                     'target': AUDIO_DEVICE_ID, 'volume': vol},
        headers=headers)


def set_vol(mode, sysmode):
    if mode == sysmode:
        return sysmode

    if mode == 'normal':
        volreq(DEFAULT_VOLUME)
    elif mode == 'lower':
        volreq(LOWER_VOLUME)
    elif mode == 'mute':
        volreq(0)

    return mode


while True:
    ret, frame = cap.read()
    frame = cv2.resize(frame, (300, 300))
    frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    if last is not None:
        diff = cv2.absdiff(last, frame)

        _, thresh = cv2.threshold(diff, 25, 255, cv2.THRESH_BINARY)

        last_th.append(np.sum(thresh))

    if (len(last_th) > MAX_HOLD_FRAMES):
        last_th = last_th[1:]

    if (len(last_th) > MOVE_EVAL_FRAME):
        movf = last_th[-MOVE_EVAL_FRAME:]
        if (np.mean(movf) > MOVE_EVAL_MEAN):
            print("!", end="", flush=True)
            last_move_time = time.time()
            mode_holder = 'normal'

    if (time.time() - last_move_time > DECIDE_LOWER_TIME and mode_holder == 'normal'):
        print("L", end="", flush=True)
        mode_holder = 'lower'
    elif (time.time() - last_move_time > DECIDE_MUTE_TIME and mode_holder == 'lower'):
        print("M", end="", flush=True)
        mode_holder = 'mute'

    sysmode = set_vol(mode_holder, sysmode)

    key = cv2.waitKey(30)
    if key == 27:
        break

    last = frame

    time.sleep(0.25)
