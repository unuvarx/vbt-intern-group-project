import { useAuth } from "../../hooks/useAuth";
import Button from "../button";
import Logo from "../logo";
import Navbar from "./navbar";
import { UserCircle2 } from "lucide-react";

export default function Header() {
    const { isAuth } = useAuth();

    return (
        <header className="containerx py-2 flex justify-between items-center">
            <Logo isLink />

            <div className="max-sm:hidden">
                <Navbar />
            </div>

            <div className="max-sm:!hidden">
                {isAuth ? (
                    <>
                        <Button
                            isLink
                            href="/profile"
                            className="!flex-center gap-2"
                        >
                            <UserCircle2 />
                            <span>Profile</span>
                        </Button>
                    </>
                ) : (
                    <Button isLink href="/auth/login">
                        Giriş Yap
                    </Button>
                )}
            </div>
        </header>
    );
}
