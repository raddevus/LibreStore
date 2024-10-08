<h1 class="display-4">What Is LibreStore?</h1>
    <p>It's FOSS (Fully Open Source Software) which provides the fastest
         and easiest and secure way to store data from your web apps.</p>
    <h2>Fastest, Easiest?  Show Me An Example</h2>
    <p>Without knowing anything else you can create post your data using the following LibreStore WebAPI call:
        https://localhost:7138/data/SaveData?key=MustBeAtLeast10Chars&data=this is the first test data&hmac=fake hmac&iv=fake iv&intent=fake-test
    </p>
    <h2>Check Out The Live Github Page</h2>
    <p>
        Check out my Github page which has additional info and the ability to save test data from your browser at: <a href="https://raddevus.github.io/LibreStore/" target="_blank">https://raddevus.github.io/LibreStore/</a>
    </p>
    <h2>How Is My Data Made Secure?</h2>
    <p>Here's what you need to know about securing your (or your user's) data.
        <ol>
            <li>All data is intended* to be encrypted on the Client side</li>
            <li>The LibreStore developer provides you with various client-side algorithms
                which will allow you to use AES256 Authenticated Encryption to insure your
                data is always encrypted with a pass-phrase that only you know.
            </li>
            <li>Authenticated Encryption insures the encrypted data has not been corrupted
                 or tampered with via HMAC (Hashed Message Authentication Code).
                 This means your client-side code _should_ generate an HMAC and securely generated 
                 random IV (Initiliazation Vector) which will be stored (in LibreStore's db) along
                 along with the data so you can verify data has not been corrupted or tampered with.
            </li>
            <li>However, if you choose not to encrypt your data and you store it as clear-text 
                in the LibreStore database, LibreStore is not responsible if your data is stolen and/or 
                used for malicious purposes.
            </li>
            <li>Of course, there will be much more explanation about how to properly encrypt your data 
                and how all of this works.
            </li>
        </ol>
    </p>
    <h2>What Technology Is LibreStore Built On</h2>
    <p>It's a .NET Core 6.x/7.x WebAPI written in C#.</p>
    <p>It uses a Sqlite Database to store the user's data.</p>
    <h2>Where Can I See the Source Code?</h2>
    <p>At my GitHub repo: 
        <a href="https://github.com/raddevus/LibreStore" target="_blank">LibreStore source at GitHub</a>
    </p>
    <h2>Are There Any Projects That Use LibreStore</h2>
    <h3>C'YaPass: The FOSS Password Generator & Manager</h3>
    <p>C'YaPass uses it to encrypt (using Authenticated Encryption & AES256, of course) the 
        user's Site-Keys when the user decides to store them for retrieval from any other system.
        The Site-Keys are encrypted with a password that only the user knows and they cannot be 
        decrypted unless the proper password is used.
        <p>You can try the <a href="https://cyapass.com/js/cya.htm" target="_blank">Web version of C'YaPass^</a> at the official site.</p>
        <p>You can get the Android app at: </p>
        <p>You can get the Windows App at: </p>
        <p>You can get the Linux App at: </p>
        <p>You can see the source code of C'YaPass built on ElectronJS which is cross-platform at: </p>
        <p>You can see all of the source code for C'YaPass at: </p>
    </p>
    