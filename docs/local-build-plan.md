# StageManager portable build

Scope approved by user: automatic window capture, multi-app scenes, whole-scene switching, exit restoration, self-contained Windows x64 ZIP.

1. Reuse upstream enumeration/WinEvent hooks, SceneRegistry merge/split, sidebar and parking journal.
2. Add a stopped lifecycle to the scene registry; block asynchronous switches after shutdown. Add regression coverage.
3. Select a fallback initial scene when the sidebar owns the foreground.
4. Restore every tracked scene on exit even if an individual window fails. Add recovery-only startup and portable data marker.
5. Run existing tests, build and publish self-contained; smoke-test WPF startup without touching user windows. Deliver ZIP with license and Chinese instructions.

Real desktop switching remains a separate interactive acceptance check; do not claim it passed from unit tests.
