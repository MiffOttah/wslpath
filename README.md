# WslPath

WslPath is a utility designed to convert between native Windows and
WSL path names, akin to Cygwin's `cygpath` utility.

It's intended for use in WSL-based scripts that interoperate with
native Windows.

The `-u` and `-w` options are based on `cygpath`'s, so this utility,
in some cases, can be a drop in replacement. This eases learning the
utility for users used to Cygwin, and allows for easier porting of
`cygpath`-based scripts to WSL.

## Usage

    wslpath [OPTION] [PATH1 [PATH2 ...]]

If no PATHs are specified, the current directory is used instead.

### Options
  * `-u`, `--unix` - Outputs UNIX/Linux paths for inside WSL
  * `-w`, `--windows` - Outputs Windows paths for outside WSL
  * `--help` - Displays this text
  * `--version` - Displays version information

### Return code

  * `0` if all paths are successfully converted
  * `1` if a path could not be converted (for example, a path within
    WSL's internal filesystem)
  * `2` for an incorrect command line
 
## Copyright/Credits

This utility was made by [Miff](https://www.miffthefox.info/). It's
made available under the [MIT License](https://opensource.org/licenses/MIT).

This utility was inspired by Cygwin's very useful `cygpath` utility,
but doesn't use any source code from it.


