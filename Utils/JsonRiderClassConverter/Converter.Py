def generate_pascal_case(word) -> str:
    return "".join(word.title() for word in word.split("_"))

lines = []
with open("genericResponseFromRider.cs", 'r') as f:
    lines = f.readlines()

with open("genericResponseFromRider.cs", 'w') as f:
    f.write("using System.Text.Json.Serialization;" + "\n\n")
    
    for line in lines:
        line = line.strip().split(" ")
        if not line or len(line) < 3 or line[1] == "class":
            f.write(str.join(" ", line) + "\n")
            continue
        
        json_property_name = f"\t[JsonPropertyName(\"{line[2]}\")]"
        f.write(json_property_name + "\n")
        word = line[2]
        word = generate_pascal_case(word)
        print(word)
        line[2] = word
        f.write("\t" + str.join(" ", line) + "\n\n")