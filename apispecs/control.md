# Home BGM System - Audio Player Control API

## Control System
```http
POST /
```

Body: json, utf-8

### Change Output Volume
```json
{
    "type": "vol",
    "target": "OutputName",
    "vol": 1
}
```

`vol` must be 0~1 float.

### Change Playing File
```json
{
    "type": "file",
    "file": "<File Path>"
}
```
