export function Button(
  props: React.ButtonHTMLAttributes<HTMLButtonElement> & { variant?: "primary" | "ghost" }
) {
  const variant = props.variant ?? "primary";

  const base =
    "h-11 rounded-xl px-5 text-sm font-medium transition-all duration-150 disabled:opacity-50";

  const styles =
    variant === "primary"
      ? "bg-blue-600 text-white hover:bg-blue-700 shadow-sm"
      : "border border-slate-300 bg-white text-slate-700 hover:bg-slate-50";

  return (
    <button {...props} className={[base, styles, props.className ?? ""].join(" ")}>
      {props.children}
    </button>
  );
}