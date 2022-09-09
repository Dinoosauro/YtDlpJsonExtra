# YtDlpJsonExtra
From the .json file generated from a yt-dlp download, merge metadata to the video.
## Configuration
In your yt-dlp script, add ` --write-info-json` before your link, so that yt-dlp will download a JSON file with lots of metadata. Then, open YtDlpJsonExtra and wait a few seconds. All the videos in the folder with a JSON file available will be parsed and metadata will be added.
